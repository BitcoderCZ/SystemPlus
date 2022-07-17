using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Systemplus.Inputs;

namespace SystemPlus.Windows
{
    public struct KeyMap
    {
        /// <summary>
        /// Scan code.
        /// </summary>
        public Key Key;

        /// <summary>
        /// Value.
        /// </summary>
        public char Value;

        /// <summary>
        /// Shift.
        /// </summary>
        public char Shift;

        /// <summary>
        /// Num.
        /// </summary>
        public char Num;

        /// <summary>
        /// Caps.
        /// </summary>
        public char Caps;

        /// <summary>
        /// Shift and Caps.
        /// </summary>
        public char ShiftCaps;

        /// <summary>
        /// Shift and Num.
        /// </summary>
        public char ShiftNum;

        /// <summary>
        /// Ctrl.
        /// </summary>
        public char Control;

        /// <summary>
        /// Ctrl and Alt.
        /// </summary>
        public char ControlAlt;

        /// <summary>
        /// Ctrl and Shift.
        /// </summary>
        public char ControlShift;

        /// <summary>
        /// Ctrl, Alt and Shift.
        /// </summary>
        public char ControlAltShift;

        /// <summary>
        /// NumLock key.
        /// </summary>
        public char NumLock;

        public KeyMap(Key key, char norm, char shift, char num, char caps, char shiftcaps, char shiftnum, char altgr, char shiftaltgr, char ctrl, char shiftctrl)
        {
            Key = key;
            Value = norm;
            Shift = shift;
            Num = num;
            Caps = caps;
            ShiftCaps = shiftcaps;
            ShiftNum = shiftnum;
            NumLock = norm;
            ControlAlt = altgr;
            Control = ctrl;
            ControlAltShift = shiftaltgr;
            ControlShift = shiftctrl;
        }

        /// <summary>
        /// Create new instance of the <see cref="KeyMapping"/> class.
        /// </summary>
        /// <param name="aScanCode">A scan code.</param>
        /// <param name="norm">Norm.</param>
        /// <param name="shift">Shift.</param>
        /// <param name="num">Num.</param>
        /// <param name="caps">Caps.</param>
        /// <param name="shiftcaps">Shift and Caps.</param>
        /// <param name="shiftnum">Shift and Num</param>
        /// <param name="altgr">Ctrl and Alt.</param>
        /// <param name="shiftaltgr">Ctrl, Alt and Shift.</param>
        /// <param name="aKey">A key.</param>
        /// <param name="numKey">NumLock key.</param>
        public KeyMap(Key key, char norm, char shift, char num, char caps, char shiftcaps, char shiftnum, char altgr, char shiftaltgr)
            : this(key, norm, shift, num, caps, shiftcaps, shiftnum, altgr, shiftaltgr, '\0', '\0')
        {
        }

        /// <summary>
        /// Create new instance of the <see cref="KeyMapping"/> class.
        /// </summary>
        /// <param name="aScanCode">A scan code.</param>
        /// <param name="norm">Norm.</param>
        /// <param name="shift">Shift.</param>
        /// <param name="num">Num.</param>
        /// <param name="caps">Caps.</param>
        /// <param name="shiftcaps">Shift and Caps.</param>
        /// <param name="shiftnum">Shift and Num</param>
        /// <param name="altgr">Ctrl and Alt.</param>
        /// <param name="aKey">A key.</param>
        /// <param name="numKey">NumLock key.</param>
        public KeyMap(Key key, char norm, char shift, char num, char caps, char shiftcaps, char shiftnum, char altgr)
            : this(key, norm, shift, num, caps, shiftcaps, shiftnum, altgr, '\0', '\0', '\0')
        {
        }

        /// <summary>
        /// Create new instance of the <see cref="KeyMapping"/> class.
        /// </summary>
        /// <param name="aScanCode">A scan code.</param>
        /// <param name="norm">Norm.</param>
        /// <param name="shift">Shift.</param>
        /// <param name="num">Num.</param>
        /// <param name="caps">Caps.</param>
        /// <param name="shiftcaps">Shift and Caps.</param>
        /// <param name="shiftnum">Shift and Num</param>
        /// <param name="aKey">A key.</param>
        public KeyMap(Key key, char norm, char shift, char num, char caps, char shiftcaps, char shiftnum)
            : this(key, norm, shift, num, caps, shiftcaps, shiftnum, '\0', '\0', '\0', '\0')
        {
        }

        /// <summary>
        /// Create new instance of the <see cref="KeyMapping"/> class.
        /// </summary>
        /// <param name="aScanCode">A scan code.</param>
        /// <param name="norm">Norm.</param>
        /// <param name="shift">Shift.</param>
        /// <param name="num">Num.</param>
        /// <param name="caps">Caps.</param>
        /// <param name="shiftcaps">Shift and Caps.</param>
        /// <param name="shiftnum">Shift and Num</param>
        /// <param name="aKey">A key.</param>
        /// <param name="numKey">NumLock key.</param>
        /*public KeyMap(Key key, char norm, char shift, char num, char caps, char shiftcaps, char shiftnum)
            : this(key, norm, shift, num, caps, shiftcaps, shiftnum, '\0')
        {
        }*/

        /// <summary>
        /// Create new instance of the <see cref="KeyMapping"/> class.
        /// </summary>
        /// <param name="aScanCode">A scan code.</param>
        /// <param name="n">All control keys char.</param>
        /// <param name="aKey">A key.</param>
        public KeyMap(Key key, char n)
            : this(key, n, n, n, n, n, n)
        {
        }
    }
}
