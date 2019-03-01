using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola
{
    /// <summary>
    /// 拓扑容器（回收）
    /// </summary>
    internal interface ITopologyContainer
    {
        void Retrieve(Topology topology);
    }
}
