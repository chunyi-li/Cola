using System;
using System.Collections.Generic;
using System.Text;

namespace Likg.Cola.Configs
{
    internal class TopologyConfig : ITopologyConfig
    {
        public static readonly string LastName = "Last#Likanggui";
        private Dictionary<int, List<string>> levels;
        private Dictionary<string, DependItem> boltDependlist;
        private DependItem mashupItem;
        private bool inited = false;//当前配置信息是否已初始化（1个配置只需初始化一次，生成拓扑时，只需使用扑信息创建对象）
        public TopologyConfig()
        {
            this.levels = new Dictionary<int, List<string>>();
            this.boltDependlist = new Dictionary<string, DependItem>();
        }

        public ITopologyConfig SetBolt(string[] previous, string name, Type boltType)
        {
            this.BoltCheck(name, boltType);
            this.boltDependlist.Add(name, new DependItem() { Name = name, Previous = previous, Type = boltType });

            return this;
        }

        public ITopologyConfig SetBolt(string[] previous, string name, Action<IInputs, IOutput> action)
        {
            this.BoltCheck(name, typeof(BoltDelegate));
            this.boltDependlist.Add(name, new DependItem() { Name = name, Previous = previous, Type = typeof(BoltDelegate), BoltAction = action });
            return this;
        }

        private void BoltCheck(string name, Type boltType)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("参数2，值错误，请设置节点名称 ");

            if (boltType == null)
                throw new ArgumentException("参数3错误，不能为null且应为IBolt类型");

            if (boltType.GetInterface(typeof(IBolt).Name, true) == null)
                throw new ArgumentException("参数3，类型错误，应为IBolt");

            if (this.boltDependlist.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("已存在名为{0}的Bolt", name));
            }
        }

        public ITopologyConfig SetMashup(string[] previous, Type mashupType)
        {
            if (mashupType == null)
                throw new ArgumentException("参数2错误，不能为null且应为IMashup类型");

            if (mashupType.GetInterface(typeof(IMashup).Name, true) == null)
            {
                throw new ArgumentException("参数2类型错误，应为IMashup类型");
            }

            if (mashupItem != null)
                throw new ArgumentException("已设定Mashup,不能重复设置");

            mashupItem = new DependItem() { Name = LastName, Previous = previous, Type = mashupType };
            return this;
        }

        public ITopologyConfig SetMashup(string[] previous, Func<IInputs, object> mashupFunction)
        {
            if (mashupItem != null)
                throw new ArgumentException("已设定Mashup,不能重复设置");

            mashupItem = new DependItem() { Name = LastName, Previous = previous, Type = typeof(MashupDelegate), MashupFunction = mashupFunction };
            return this;
        }

        internal Topology Build(ITopologyContainer container)
        {
            if (!this.inited)
            {
                this.Init();
                this.inited = true;
            }

            return new Topology(this.levels, this.boltDependlist, this.mashupItem, container);
        }

        private void Init()
        {
            foreach (var item in this.boltDependlist)
            {
                if (item.Value.Previous == null)
                {
                    continue;
                }

                this.SetDependOn(item.Value);
            }

            if (this.mashupItem == null)
                throw new Exception("请在设置中增加汇聚点Mashup");

            this.SetDependOn(this.mashupItem);
            this.mashupItem.BuilLevel();

            //检查拓扑设置，是否有Bolt没到达汇聚点
            foreach (var item in this.boltDependlist.Values)
            {
                if (item.Level < 0)
                    throw new Exception(string.Format("名称为{0}的Bolt不能直接或间接到达汇聚节点", item.Name));

                List<string> typeList;
                if (!this.levels.TryGetValue(item.Level, out typeList))
                {
                    typeList = new List<string>();
                    this.levels.Add(item.Level, typeList);
                }

                typeList.Add(item.Name);
            }

        }

        private void SetDependOn(DependItem bolt)
        {
            if (bolt.Previous == null || bolt.Previous.Length < 1)
                return;

            DependItem dependBolt;
            foreach (var previou in bolt.Previous)
            {
                if (boltDependlist.TryGetValue(previou, out dependBolt))
                {
                    bolt.AddDependOn(dependBolt);
                }
            }
        }
    }
}
