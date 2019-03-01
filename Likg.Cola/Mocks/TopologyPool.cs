using Likg.Cola.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola
{
#if DEBUG
    public partial class TopologyPool
    {
        public MockConfig Mockconfig { get { return new MockConfig(); } }
        public class MockConfig
        {
            public void MockSetExistName(string name)
            {
                TopologyPool.Instance.configs = new Dictionary<string, TopologyConfig>();
                TopologyPool.Instance.configs.Add(name, new TopologyConfig());
            }

            public void MockClear()
            {
                TopologyPool.Instance.configs.Clear();
                TopologyPool.Instance.topologys.Clear();
            }
        }
    }

#endif
}
