using Abc;
using Abc.Visuals;

namespace WpfControls.WpfDrawingVisualTreeInternals
{
    internal abstract class WpfDrawCommandSyncer : NativeVisualSyncer
    {
        protected WpfDrawCommandSyncer(AbcVisual abcVisual, bool hasContextualListeners)
            : base(abcVisual, hasContextualListeners)
        {
        }
    }
}
