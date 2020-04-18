using Abc.Primitives;
using Abc.Visuals;
using System;

namespace WpfControls.WpfDrawingVisualTreeInternals
{
    internal class WpfCanvasSyncer : WpfDrawCommandSyncer
    {
        public WpfCanvasSyncer(AbcCanvas abcVisual)
            : base(abcVisual, hasContextualListeners: true)
        {
        }

        internal override AbcSize Measure(AbcMeasureContext context)
        {
            throw new NotImplementedException();
        }
    }
}
