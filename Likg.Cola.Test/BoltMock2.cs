using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    public class BoltMock2 : IBolt
    {
        public void Execute(IInputs input, IOutput output)
        {
            var p1 = (int)input.GetValue();
            System.Diagnostics.Debug.WriteLine("BlotMock2" + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(200);
            System.Diagnostics.Debug.WriteLine("BlotMock2" + Thread.CurrentThread.ManagedThreadId);
            output.Emit("out2", p1 + 20);
        }
    }
}
