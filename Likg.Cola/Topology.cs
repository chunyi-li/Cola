using Likg.Cola.Channels;
using Likg.Cola.Configs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Likg.Cola
{
    /// <summary>
    /// 执行拓扑的
    /// </summary>
    public class Topology : ITopologyRelease
    {
        /// <summary>
        /// 拓扑级联
        /// </summary>
        private Dictionary<int, List<BoltExcuter>> levelList;
        /// <summary>
        /// 用于关联输入输出
        /// </summary>
        private Dictionary<string, BoltExcuter> boltList;
        private MashupExcuter mashupExcuter;
        private UserInput userInput;//用户输入的参数

        /// <summary>
        /// 处理回收
        /// </summary>
        private ITopologyContainer container;

        internal Topology(Dictionary<int, List<string>> levels, Dictionary<string, DependItem> list, DependItem mashupItem, ITopologyContainer container)
        {
            this.userInput = new UserInput();
            this.boltList = new Dictionary<string, BoltExcuter>();
            this.container = container;
            this.levelList = new Dictionary<int, List<BoltExcuter>>();
            foreach (var level in levels)
            {
                levelList.Add(level.Key, this.CreateLevel(level.Value, list));
            }

            IInputs mashupInput = null;
            if (levels.Count > 0)
            {
                mashupInput = new InputCollection(mashupItem.Previous, boltList);
            }
            else
            {
                mashupInput = this.userInput;
            }
            var mashup = (IMashup)Activator.CreateInstance(mashupItem.Type);
            if (mashup is MashupDelegate)
            {
                ((MashupDelegate)mashup).SetAction(mashupItem.MashupFunction);
            }

            this.mashupExcuter = new MashupExcuter(mashup, mashupInput);
        }

        public Task<object> Execute(params KeyValuePair<string, object>[] values)
        { 
            this.userInput.SetInput(values);
            var task = new Task<object>(() =>
            {
                try
                {
                    foreach (var level in this.levelList)
                    {
                        this.RunLevel(level.Value);
                    }

                    return mashupExcuter.Execute();

                }
                finally
                {
                    //执行完毕需要回到队列中
                    this.container.Retrieve(this);
                }
            });

            task.Start();
            return task;
            //return result;
        }

        private void RunLevel(IList<BoltExcuter> list)
        {
            foreach (var item in list)
            {
                item.Execute();
            }
        }

        private List<BoltExcuter> CreateLevel(List<string> levelList, Dictionary<string, DependItem> dependList)
        {
            var excuteList = new List<BoltExcuter>();
            foreach (var item in levelList)
            {
                var boltInfo = dependList[item];
                var bolt = (IBolt)Activator.CreateInstance(boltInfo.Type);

                if (bolt is BoltDelegate)
                {
                    ((BoltDelegate)bolt).SetAction(boltInfo.BoltAction);
                }

                IInputs inputs = null;
                if (boltInfo.Previous != null)
                {
                    inputs = new InputCollection(boltInfo.Previous, boltList);
                }
                else
                {
                    inputs = this.userInput;
                }

                var boltExcuter = new BoltExcuter(bolt, inputs);
                this.boltList.Add(item, boltExcuter);
                excuteList.Add(boltExcuter);
            }

            return excuteList;
        }

        void ITopologyRelease.Clear()
        {
            foreach (var item in boltList)
            {
                item.Value.Clear();
            }

            userInput.Clear();
        }
    }
}
