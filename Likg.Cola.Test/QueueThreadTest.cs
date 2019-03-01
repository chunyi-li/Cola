using System;
using System.Threading;
using Likg.Cola.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Likg.Cola.Test
{
    [TestClass]
    public class QueueThreadTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var qt = new QueueThread<int>((item) =>
            {
                System.Diagnostics.Debug.WriteLine(item);
                Thread.Sleep(1000);
            });

            qt.Start(8);
            for (int i = 0; i < 100; i++)
            {
               // Thread.Sleep(100);
                qt.Enqueue(i);
            }

            Thread.Sleep(40000); 
            qt.WaitEnd();
        }
    }
}
