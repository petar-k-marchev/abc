namespace Abc.Visuals
{
    internal interface IAbcControl
    {
        NativeVisualTree VisualTree { get; set; }
        IAbcVisual ControlRoot { get; }
    }
}
