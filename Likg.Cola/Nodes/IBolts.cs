using System;
using System.Collections.Generic;
using System.Text;

namespace Likg.Cola
{
    /// <summary>
    /// 可以包含0到多个
    /// </summary>
    public interface IBolt
    { 

        void Execute(IInputs input, IOutput output);
    }
}
