using Abc.Visuals;

namespace Abc.Controls
{
    internal interface IAbcControl
    {
        AbcVisualTree VisualTree { get; set; }
        IAbcVisual Root { get; }

        void OnRootMeasureInvalidated();
    }

    internal static class AbcControlContextualProperties
    {
        internal static readonly AbcContextualPropertyKey ControlPropertyKey = new AbcContextualPropertyKey();
    }
}
