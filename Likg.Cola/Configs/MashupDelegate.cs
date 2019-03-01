using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola.Configs
{
    /// <summary>
    /// 支持Mashup方法委托的注册
    /// </summary>
    internal class MashupDelegate : IMashup
    {
        Func<IInputs, object> action;
        public MashupDelegate()
        {

        }

        internal void SetAction(Func<IInputs, object> action)
        {
            this.action = action;
        }

        public object Execute(IInputs inputs)
        {
            return this.action(inputs);
        }
    }
}
