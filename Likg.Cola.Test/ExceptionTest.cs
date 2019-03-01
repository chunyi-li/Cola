using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Likg.Cola.Test
{
    [TestClass]
    public class ExceptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void TestMethod1()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(null, typeof(MashupExceptionMock));
            var r = TopologyPool.Instance.BuildTopology("test").Execute(null);
            var rs = r.Result;
            var e = r.Exception;
        }

        /// <summary>
        /// 测试重用(抛异常后可收仍可重用)
        /// </summary>
        [TestMethod]
        public void Execute2()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(new string[] { "b", "c" }, typeof(MashupMock1));
            config.SetBolt(null, "a", typeof(BoltExceptionMock));
            config.SetBolt(new string[] { "a" }, "b", typeof(BoltMock2));
            config.SetBolt(null, "c", typeof(BoltMock4));
            var b = DateTime.Now;
            var r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("test1", 10), new KeyValuePair<string, object>("test2", 10));
            try
            {
                var result = r.Result;
                var ex = r.Exception;
            }
            catch (Exception ex)
            {
                int a = 0;
             //   throw;
            }

             r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("test1", 10), new KeyValuePair<string, object>("test2", 10));
            var result2 = r.Result;
            Assert.IsTrue((DateTime.Now - b).TotalMilliseconds < 600);
        }
    }
}
