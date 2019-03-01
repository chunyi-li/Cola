using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola
{
    internal class DependItem
    {
        public string Name { get; set; }

        public string[] Previous { get; set; }

        public Type Type { get; set; }

        /// <summary>
        /// 只有在使用委托的方式时才有效
        /// </summary>
        public Action<IInputs, IOutput> BoltAction { get; set; }

        public Func<IInputs,object> MashupFunction { get; set; }
        /// <summary>
        /// 执行级别
        /// </summary>
        public int Level { get; private set; }

        private List<DependItem> DependOns;

        public DependItem()
        {
            this.Level = -1;
            this.DependOns = new List<DependItem>();
        }

        public void AddDependOn(DependItem b)
        {
            this.DependOns.Add(b);
        }

        public void BuilLevel()
        {
            if (this.DependOns.Count < 1)
            {
                this.Level = 0;
                return;
            }

            foreach (var item in this.DependOns)
            {
                item.BuilLevel();
                if (item.Level > this.Level)
                {
                    this.Level = item.Level + 1;
                }
            }
        }
    }
}
