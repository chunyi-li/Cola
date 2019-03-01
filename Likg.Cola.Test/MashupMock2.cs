using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    public class MashupMock2 : IMashup
    {
        public object Execute(IInputs inputs)
        {
            var p1=(int)inputs.GetValue("out1");
            var p2 = (int)inputs.GetValue("out2");
            var p3 = (int)inputs.GetValue("out4");
            Thread.Sleep(100);
            return p1+p2+p3;
        } 
    }
}
