using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola.Configs
{
    /// <summary>
    /// 支持Bolt方法委托的注册
    /// </summary>
    internal class BoltDelegate : IBolt
    {
        Action<IInputs, IOutput> boltAction;

        public BoltDelegate()
        { }
        public void SetAction(Action<IInputs, IOutput> boltAction)
        {
            this.boltAction = boltAction;
        }

        public void Execute(IInputs input, IOutput output)
        {
            this.boltAction(input, output);
        }
    }
}
