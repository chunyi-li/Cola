using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    public class BoltExceptionMock : IBolt
    {
        int i = 0;
        public void Execute(IInputs input, IOutput output)
        {
            if (i == 0)
            {
                i++;
                throw new NotImplementedException();
            }
            else {
                var p1 = (int)input.GetValue();
                System.Diagnostics.Debug.WriteLine("BlotMock1" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(100);
                System.Diagnostics.Debug.WriteLine("BlotMock1" + Thread.CurrentThread.ManagedThreadId);
                output.Emit("out1", p1 + 10);
            }

            
        }
    }
}
