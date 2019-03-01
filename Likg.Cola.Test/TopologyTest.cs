using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Likg.Cola.Test
{
    /// <summary>
    /// 拓扑执行测试
    /// 1、异步执行
    /// 2、同步执行
    /// 2、返回正确值
    /// 3、拓扑中任何一个节点出现异常，均中断后续所有节点的执行
    /// 
    /// 拓扑设置测试
    /// 确保所有Bolt都有一至多个下级处理节点
    /// 
    /// Bolt有多个下级处理节点
    /// Bolt有一个下级处理节点
    /// Bolt无下级处理节
    /// 
    /// Mashup不是所有指定上级节点都存在
    /// Mashup可以为空
    /// 
    /// 
    /// 不同Bolt输出的值，不能有相同的KEY
    /// 
    /// </summary>
    [TestClass]
    public class TopologyTest
    {
        /// <summary>
        /// 仅一个并发,并发线程与调用线程，分别异步执行
        /// </summary>
        [TestMethod]
        public void Execute_WhenAwait_ThenAsync()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(null, typeof(MashupMock1));

            var bTime = DateTime.Now;
            var r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("test", 10));
            Thread.Sleep(100);
            var timeSpan = (DateTime.Now - bTime).TotalMilliseconds;
            Assert.AreEqual(r.Result, 110);
            Assert.IsTrue(timeSpan < 200);
        }

        /// <summary>
        /// 串行执行
        /// </summary>
        [TestMethod]
        public void Execute_WhenAwait_ThenAsync2()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(new string[] { "a" }, typeof(MashupMock1));
            config.SetBolt(null, "a", typeof(BoltMock1));
            var r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("test1", 10));
            Assert.AreEqual(r.Result, 120);

        }

        /// <summary>
        /// 串行执行
        /// </summary>
        [TestMethod]
        public void Execute_WhenAwait_ThenAsync3()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(new string[] { "a", "b", "d" }, typeof(MashupMock1));
            config.SetBolt(null, "a", typeof(BoltMock1));
            config.SetBolt(null, "b", typeof(BoltMock2));
            config.SetBolt(null, "c", typeof(BoltMock3));
            config.SetBolt(new string[] { "c" }, "d", typeof(BoltMock4));
            var b = DateTime.Now;
            var r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("test1", 10), new KeyValuePair<string, object>("test2", 10));

            var aaa = (DateTime.Now - b).TotalMilliseconds;
            Assert.AreEqual(r.Result, 70);

        }

        ///    a       c
        ///    |       /
        ///    b      /
        ///    \     /
        ///       汇取点
        ///       
        ///   A、B串行，C并行
        /// </summary>
        [TestMethod]
        public void Execute2()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(new string[] { "b", "c" }, typeof(MashupMock1));
            config.SetBolt(null, "a", typeof(BoltMock1));
            config.SetBolt(new string[] { "a" }, "b", typeof(BoltMock2));
            config.SetBolt(null, "c", typeof(BoltMock4));
            var b = DateTime.Now;
            var r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("test1", 10), new KeyValuePair<string, object>("test2", 10));
            var a = r.Result;
            Assert.IsTrue((DateTime.Now - b).TotalMilliseconds < 600);
        }

        ///测试拓扑的重复使用
        ///一个Topology使用完后，会回归到TopologyPool
        [TestMethod]
        public void Execute3()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(new string[] { "b", "c" }, typeof(MashupMock1));
            config.SetBolt(null, "a", typeof(BoltMock1));
            config.SetBolt(new string[] { "a" }, "b", typeof(BoltMock2));
            config.SetBolt(null, "c", typeof(BoltMock4));
            var b = DateTime.Now;
            var r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("test1", 10), new KeyValuePair<string, object>("test2", 10));
            var a = r.Result;
            Assert.IsTrue((DateTime.Now - b).TotalMilliseconds < 600);
        }

        [TestMethod]
        public async void Execute4()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(new string[] { "b", "c" }, inputs =>
            {
                var p1 = (int)inputs.GetValue("out2");
                var p2 = (int)inputs.GetValue("out4");
                return p1 + p2;
            });

            config.SetBolt(null, "a", (inputs, output) =>
            {
                var p1 = (int)inputs.GetValue();
                output.Emit("out1", p1 + 10);
            });

            config.SetBolt(new string[] { "a" }, "b", (inputs, output) =>
            {
                var p1 = (int)inputs.GetValue();
                output.Emit("out2", p1 + 20);
            });

            config.SetBolt(null, "c", (inputs, output) =>
            {
                var p1 = (int)inputs.GetValue();
                Thread.Sleep(10000);
                output.Emit("out4", p1 + 40);
            });

            var b = DateTime.Now;
            var r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("out1", 10), new KeyValuePair<string, object>("out2", 10));
            var a = r.Result;
            Assert.AreEqual(90, a);
        }
        // Likg.Cola.Configs
    }
}
