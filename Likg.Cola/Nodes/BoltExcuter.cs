using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola
{
    /// <summary>
    /// 
    /// </summary>
    internal class BoltExcuter
    {
        // private BoltExcuter[] previouExcuters;
        internal Output output;

        private IInputs input;

        private IBolt bolt;

        public BoltExcuter(IBolt bolt, IInputs input)
        {
            this.bolt = bolt;
            this.input = input;
            this.output = new Output();
        }

        internal void Execute()
        {
            var task = new Task(() =>
            {
                if (input is InputCollection)
                {
                    Task.WaitAll(((InputCollection)input).Tasks);//所有前置任务已完成
                }
                
                this.bolt.Execute(this.input, this.output);
            });

            task.Start();
            ((ISetTask)output).SetTask(task);
        }

        /// <summary>
        /// 清除输出
        /// </summary>
        internal void Clear()
        {
            output.Clear();
        }
    }
}
