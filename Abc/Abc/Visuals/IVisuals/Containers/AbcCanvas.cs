namespace Abc.Visuals
{
    internal interface IAbcCanvas : IAbcVisualsContainer
    {
    }

    internal static class AbcCanvasContextualProperties
    {
        internal static readonly AbcContextualPropertyKey LayoutSlotPropertyKey = new AbcContextualPropertyKey();
    }
}
