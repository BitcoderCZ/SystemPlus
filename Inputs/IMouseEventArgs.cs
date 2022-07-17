using SystemPlus.Vectors;

namespace Systemplus.Inputs
{
    internal interface IMouseEventArgs
    {
        Vector2D Position { get; }

        MouseButtons Buttons { get; }

        int WheelDelta { get; }

        int ClickCount { get; }
    }
}
