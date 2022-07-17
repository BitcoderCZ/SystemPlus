using System;

namespace Systemplus.Inputs
{
    /// <summary>
    /// Flags representing modifiers.
    /// </summary>
    [Flags]
    public enum Modifiers
    {
        None =
            0b_0000_0000,

        Control =
            0b_0000_0001,

        Alt =
            0b_0000_0010,

        Shift =
            0b_0000_0100,

        Windows =
            0b_0000_1000,

        CapsLock =
            0b_0001_0000,

        NumbLock =
            0b_0010_0000,
    }
}
