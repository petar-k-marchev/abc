using Abc.Visuals;

namespace Abc.Controls
{
    internal interface IAbcControl
    {
        AbcVisualTree VisualTree { get; set; }
        IAbcVisual Root { get; }
    }
}
