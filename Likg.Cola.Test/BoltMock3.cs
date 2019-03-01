using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    public class BoltMock3 : IBolt
    {
        public void Execute(IInputs input, IOutput output)
        {
            var p1 = (int)input.GetValue();
            System.Diagnostics.Debug.WriteLine("BlotMock3" + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(300);
            System.Diagnostics.Debug.WriteLine("BlotMock3" + Thread.CurrentThread.ManagedThreadId);
            output.Emit("out3", p1 + 30);
        }
    }
}
