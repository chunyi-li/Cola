using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    public class BoltMock1 : IBolt
    {
       public static Dictionary<int, int> count=new Dictionary<int, int>();
        public void Execute(IInputs input, IOutput output)
        {
            var p1=(int)input.GetValue();
            lock (count)
            {
                int c = 0;
                if (count.TryGetValue(Thread.CurrentThread.ManagedThreadId, out c))
                {
                    c++;
                }
                else
                {
                    count.Add(Thread.CurrentThread.ManagedThreadId, 1);
                }
            }

           // System.Diagnostics.Debug.WriteLine("BlotMock1 " + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(3000);
           // System.Diagnostics.Debug.WriteLine("BlotMock1" + Thread.CurrentThread.ManagedThreadId);
            output.Emit("out1", p1+10);
        }
    }
}
