using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Likg.Cola
{
    internal class MashupExcuter
    {

        private IInputs input;
        private IMashup mashup;

        public MashupExcuter(IMashup mashup, IInputs input)
        {
            this.mashup = mashup;
            this.input = input;
        }

        internal object Execute()
        {
            if (input is InputCollection)
            { 
                Task.WaitAll(((InputCollection)input).Tasks);//所有前置任务已完成 
            }

            return this.mashup.Execute(this.input);
        }
    }
}
