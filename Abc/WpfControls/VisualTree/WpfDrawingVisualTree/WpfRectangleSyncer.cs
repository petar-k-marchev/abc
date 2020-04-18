using Abc.Primitives;
using Abc.Visuals;
using System;

namespace WpfControls.WpfDrawingVisualTreeInternals
{
    internal class WpfRectangleSyncer : WpfDrawCommandSyncer
    {
        public WpfRectangleSyncer(AbcRectangle abcVisual)
            : base(abcVisual, hasContextualListeners: true)
        {
        }

        internal override AbcSize Measure(AbcMeasureContext context)
        {
            throw new NotImplementedException();
        }
    }
}
