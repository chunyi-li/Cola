using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Likg.Cola.Test
{
    /// <summary>
    /// 拓扑配置测试
    /// </summary>
    [TestClass]
    public class ConfigTest
    {
        /// <summary>
        /// 成功他建
        /// </summary>
        [TestMethod]
        public void CreateConfig_WhenNotExit_ThenCreate()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            Assert.IsNotNull(config);
        }

        /// <summary>
        /// 已存在重名的配置，抛异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateConfig_WhenExit_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            TopologyPool.Instance.Mockconfig.MockSetExistName("test");
            var config = TopologyPool.Instance.CreateConfig("test");
        }

        /// <summary>
        /// 汇聚点的类型不继承自IMashup,抛异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateConfig_WhenMashupTypeNotImpleIMashup_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(null, typeof(ConfigTest));
        }


        /// <summary>
        /// 汇聚点重复设置,抛异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateConfig_WhenMashupExist_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(null, typeof(MashupMock1));
            config.SetMashup(null, typeof(MashupMock1));
        }

        /// <summary>
        /// 设置汇聚点成功
        /// </summary>
        [TestMethod]
        public void CreateConfig_WhenImplIMashup_ThenSucess()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetMashup(null, typeof(MashupMock1));
        }

        /// <summary>
        /// 节点名称为空节符串,抛异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateConfig_WhenNameEmpty_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetBolt(null, "", typeof(BoltMock1));
        }

        /// <summary>
        /// 节点名称为Null,抛异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateConfig_WhenNameNull_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetBolt(null, null, typeof(BoltMock1));
        }

        ///// <summary>
        ///// Type为Null,抛异常
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void CreateConfig_WhenTypeNull_ThenException()
        //{
        //    TopologyPool.Instance.Mockconfig.MockClear();
        //    var config = TopologyPool.Instance.CreateConfig("test");
        //    config.SetBolt(null, "t1", null);
        //}


        /// <summary>
        /// 并行节点的类型不继承自IBolt,抛异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateConfig_WhenNotImpleIBolt_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetBolt(null, "t1", typeof(ConfigTest));
        }

        /// <summary>
        /// 并行节点设置成功
        /// </summary>
        [TestMethod]
        public void CreateConfig_WhenImpleIBolt_ThenSucess()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetBolt(null, "t1", typeof(BoltMock1));
        }

        //没置汇聚点,异常
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateConfig_WhenNotMashup_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetBolt(null, "t1", typeof(BoltMock1));
            TopologyPool.Instance.BuildTopology("test").Execute(null);
        }

        //Bolt没关汇聚到汇聚点,异常
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateConfig_WhenBoltNotToMashup_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetBolt(null, "t1", typeof(BoltMock1));
            config.SetMashup(null, typeof(MashupMock2));
            TopologyPool.Instance.BuildTopology("test").Execute(null);
        }

        /// <summary>
        /// 关联不存名称的节点，抛异常
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateConfig_WhenNotRelation_ThenException()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetBolt(null, "t1", typeof(BoltMock1));
            config.SetBolt(null, "t2", typeof(BoltMock2));
            config.SetMashup(new string[] { "t1", "t2", "t3" }, typeof(MashupMock2));
            TopologyPool.Instance.BuildTopology("test").Execute(null);
        }

        //Bolt与Mashup设置正常
        [TestMethod]
        public void CreateConfig_WhenHaveMashup_ThenSucess()
        {
            TopologyPool.Instance.Mockconfig.MockClear();
            var config = TopologyPool.Instance.CreateConfig("test");
            config.SetBolt(null, "t1", typeof(BoltMock1));
            config.SetBolt(null, "t2", typeof(BoltMock2));
            config.SetMashup(new string[] { "t1", "t2" }, typeof(MashupMock1));
            var r = TopologyPool.Instance.BuildTopology("test").Execute(new KeyValuePair<string, object>("a", 0)); 
           Assert.AreEqual(110,r.Result);
        }
    }
}
