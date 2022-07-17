using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Systemplus.Inputs;

namespace SystemPlus.Windows
{
    public abstract class KeyboardLayout : IKeyboardLayout
    {
        /// <summary>
        /// If true and key is not fount in <see cref="map"/> and is considered a letter it will be automaticly created
        /// </summary>
        protected bool AutoImplementLetters { get; }

        protected abstract Dictionary<Key, KeyMap> map { get; }

        /*[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);*/

        public char Map(Key key, Modifiers modifiers)
        {
            throw new NotImplementedException();
        }
    }
}
