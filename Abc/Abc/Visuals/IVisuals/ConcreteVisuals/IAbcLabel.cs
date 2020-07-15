namespace Abc.Visuals
{
    internal interface IAbcLabel : IAbcVisual
    {
        string Text { get; set; }
        double FontSize { get; set; }
    }
}
