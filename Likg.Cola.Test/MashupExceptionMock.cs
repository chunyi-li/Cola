using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Likg.Cola.Test
{
    public class MashupExceptionMock : IMashup
    {
        public object Execute(IInputs inputs)
        {
            Thread.Sleep(1000);
            throw new NotImplementedException();
        }
    }
}
