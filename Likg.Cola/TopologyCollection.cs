using Likg.Cola.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Likg.Cola
{
    /// <summary>
    /// 拓扑集合,用于管理相同设置的拓扑，回收重复利用，减少重新构建对象
    /// </summary>
    internal class TopologyCollection : ITopologyContainer
    {
        private TopologyConfig config;
        private Queue<Topology> queue;
        public TopologyCollection(TopologyConfig config)
        {
            this.queue = new Queue<Topology>();
            this.config = config;
        }
        public Topology GetTopology()
        {
            lock (this)
            {
                if (this.queue.Count > 0)
                {
                    return this.queue.Dequeue();
                }

                return config.Build(this);
            }
        }

        /// <summary>
        /// 回收拓扑
        /// </summary>
        /// <param name="topology"></param>
        public void Retrieve(Topology topology)
        {
            //回收前清理执行过程中产生的结果对象
            ((ITopologyRelease)topology).Clear();

            lock (this)
            {
                this.queue.Enqueue(topology);
            }
        }
    }
}
