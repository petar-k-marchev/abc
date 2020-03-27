using Abc.Visuals;

namespace Abc
{
    internal abstract class AbcVisualTree
    {
        internal AbcVisual root;

        internal abstract AbcSize CalculateDesiredSize(AbcMeasureContext context);
    }
}
