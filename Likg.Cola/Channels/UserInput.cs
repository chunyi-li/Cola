using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola.Channels
{
    /// <summary>
    /// 用户输入的参数
    /// </summary>
    internal class UserInput : IInputs
    {
        private Dictionary<string, object> input;

        public UserInput()
        {
            this.input = new Dictionary<string, object>();
        }
        public void SetInput(KeyValuePair<string, object>[] values)
        {
            if (values == null || values.Length < 1)
                return;

            for (int i = 0; i < values.Length; i++)
            {
                var item = values[i];
                if (string.IsNullOrEmpty(item.Key))
                    throw new ArgumentException(string.Format("第{0}个参数不符合要求，名称不能为null或空字符串", i + 1));

                if (this.input.ContainsKey(item.Key))
                    throw new ArgumentException(string.Format("第{0}个参数不符合要求,已存在名称为{1}的参数", i + 1, item.Key));

                this.input.Add(item.Key, item.Value);
            }
        }

        public object GetValue(string key)
        {
            object obj = null;
            input.TryGetValue(key, out obj);
            return obj;
        }

        public object GetValue()
        {
            return input.Values.FirstOrDefault();
        }

        public void Clear()
        {
            this.input.Clear();
        }
    }
}
