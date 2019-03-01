using System;
using System.Collections.Generic;
using System.Text;

namespace Likg.Cola
{
    public interface IOutput
    {
        void Emit(string id, object value);
    }
}
