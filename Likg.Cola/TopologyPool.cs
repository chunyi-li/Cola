using Likg.Cola.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Likg.Cola
{
    /// <summary>
    /// 拓扑池，管理所有拓扑初始化、回收、重用
    /// </summary>
    public partial class TopologyPool : SingleInstance<TopologyPool>
    {
        private Dictionary<string, TopologyConfig> configs;
        private Dictionary<string, TopologyCollection> topologys;

        private TopologyPool()
        {
            this.configs = new Dictionary<string, TopologyConfig>();
            this.topologys = new Dictionary<string, TopologyCollection>();
        }

        /// <summary>
        /// 创建拓扑配置
        /// </summary>
        /// <returns></returns>
        public ITopologyConfig CreateConfig(string name)
        {
            TopologyConfig config;
            if (!this.configs.TryGetValue(name, out config))
            {
                config = new TopologyConfig();
                this.configs.Add(name, config);
                return config;
            }

            throw new ArgumentException(string.Format("已存在名为{0}的配置", name));
        }

        /// <summary>
        /// 是否包含了该名称的配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        public bool ContainerName(string name)
        {
            return this.configs.ContainsKey(name);
        }

        public Topology BuildTopology(string name)
        {
            TopologyCollection tp;
            lock (this)
            {
                if (!topologys.TryGetValue(name, out tp))
                {
                    TopologyConfig config;
                    if (!configs.TryGetValue(name, out config))
                        throw new Exception(string.Format("不存在名称为{0}的拓扑配置", name));

                    tp = new TopologyCollection(config);
                    topologys.Add(name, tp);
                }
            }

            return tp.GetTopology();
        }
    }
}
