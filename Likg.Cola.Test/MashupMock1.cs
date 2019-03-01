using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    public class MashupMock1 : IMashup
    {
        public object Execute(IInputs inputs)
        {
            var p1 = (int)inputs.GetValue();           
            //Thread.Sleep(100);
            return p1+100;
        } 
    }
}
