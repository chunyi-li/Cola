using System;
using System.Collections.Generic;
using System.Text;

namespace Likg.Cola
{
    public interface IInputs
    {
        object GetValue(string key);

        object GetValue();
    }
}
