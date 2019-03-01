using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola
{
    /// <summary>
    /// 多路输入
    /// </summary>
    internal class InputCollection : IInputs
    {
        /// <summary>
        /// 上级的输出，组合成输入
        /// </summary>
        private List<Output> outputList = null;

        internal Task[] Tasks
        {
            get
            {
                var taskList = new List<Task>();
                foreach (var item in outputList)
                {
                    taskList.Add(item.BelongTask);
                }

                return taskList.ToArray();
            }
        }

        public InputCollection(string[] previous, Dictionary<string, BoltExcuter> boltList)
        {
            this.outputList = new List<Output>();
            if (previous != null)
            {
                foreach (var item in previous)
                {
                    BoltExcuter boltExecuter = null;
                    if (!boltList.TryGetValue(item, out boltExecuter))
                        throw new Exception(string.Format("不存名为{0}的Bolt",item));

                    this.outputList.Add(boltList[item].output);
                }
            }
        }

        public object GetValue(string key)
        {
            foreach (var item in outputList)
            {
                var value = item.GetValue(key);
                if (value != null)
                    return value;
            }

            return null;
        }

        /// <summary>
        /// 取第一个
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            foreach (var item in outputList)
            {
                var value = item.GetValue();
                if (value != null)
                    return value;
            }

            return null;
        }

        public void Add(Output output)
        {
            this.outputList.Add(output);
        }

    }
}
