using System;
using System.Collections.Generic;
using System.Text;

namespace Likg.Cola
{
    /// <summary>
    /// 拓扑中至少需要一个聚合点,且是最终节点
    /// </summary>
    public interface IMashup
    {
        object Execute(IInputs inputs);
    }
}
