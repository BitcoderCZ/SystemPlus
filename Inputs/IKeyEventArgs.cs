namespace Systemplus.Inputs
{
    /// <summary>
    /// Key event arguments.
    /// </summary>
    internal interface IKeyEventArgs
    {
        /// <inheritdoc cref="System.Windows.Input.Key"/>
        Key Key { get; }

        /// <inheritdoc cref="Modifiers"/>
        Modifiers Modifiers { get; }
    }
}
