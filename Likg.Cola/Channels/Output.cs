using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola
{
    internal class Output : IOutput, ISetTask
    {
        private Dictionary<string, object> values;

        /// <summary>
        /// 所属的Task，用于等待该Task完成
        /// </summary>
        internal Task BelongTask { get; private set; }

        public Output()
        {
            this.values = new Dictionary<string, object>();
        }
        public object GetValue(string key)
        {
            object value = null;
            this.values.TryGetValue(key, out value);
            return value;
        }

        public object GetValue()
        {
            return this.values.Values.FirstOrDefault();
        }

        public void Emit(string key, object value)
        {
            values.Add(key, value);
        }

        public void Clear()
        {
            this.values.Clear();
        }

        void ISetTask.SetTask(Task task)
        {
            this.BelongTask = task;
        }

    }

    internal interface ISetTask
    {
        void SetTask(Task task);
    }
}
