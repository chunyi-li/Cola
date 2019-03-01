using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    /// <summary>
    /// 并发测试
    /// </summary>
    [TestClass]
    public class ConcurrentTest
    {
        [TestMethod]
        public void Te()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(new string[] { "a" }, typeof(MashupMock1));
            config.SetBolt(null, "a", typeof(BoltMock1));
            Task<object> r = null;
            var b= DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("test1", 10));
                //Thread.Sleep(10);
            }
            //  Assert.AreEqual(r.Result, 120);

            var ss = r.Result;
            var span = (DateTime.Now - b).TotalMilliseconds;
            var ddd= BoltMock1.count;
        }
    }
}
