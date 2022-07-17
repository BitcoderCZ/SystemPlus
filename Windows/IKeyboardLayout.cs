using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Systemplus.Inputs;

namespace SystemPlus.Windows
{
    public interface IKeyboardLayout
    {
        char Map(Key key, Modifiers modifiers);
    }
}
