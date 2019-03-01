using System;
using System.Collections.Generic;
using System.Text;

namespace Likg.Cola.Configs
{
    public interface ITopologyConfig
    {
        /// <summary>
        /// 设置计算节点,可以有0至多个
        /// </summary>
        /// <param name="previous">前一个或多个节点，无前节点请设为null</param>
        /// <param name="bolts">当前节点名称</param>
        /// <param name="bolts">当前节点类型</param>
        /// <returns></returns>
        ITopologyConfig SetBolt(string[] previous, string name, Type boltType);

        /// <summary>
        /// 设置聚合点，有且只有一个，且为最终节点
        /// </summary>
        /// <param name="previous">指定哪些节点可以到达汇聚节点</param>
        /// <param name="mashup"></param>
        /// <returns></returns>
        ITopologyConfig SetMashup(string[] previous, Type mashupType);

        ITopologyConfig SetMashup(string[] previous, Func<IInputs, object> mashupFunction);

        ITopologyConfig SetBolt(string[] previous, string name, Action<IInputs, IOutput> action);
    }
}
