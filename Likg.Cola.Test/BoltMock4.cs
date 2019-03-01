using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    public class BoltMock4 : IBolt
    {
        public void Execute(IInputs input, IOutput output)
        {
            var p1 = (int)input.GetValue();
            System.Diagnostics.Debug.WriteLine("BlotMock4" + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(400);
            System.Diagnostics.Debug.WriteLine("BlotMock4" + Thread.CurrentThread.ManagedThreadId);
            output.Emit("out4", p1 + 40); 
        }
    }
}
