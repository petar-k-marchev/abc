using Abc.Visuals;
using System.Windows.Controls;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfCanvasSyncer : WpfVisualSyncer
    {
        private readonly Canvas nativeCanvas;

        public WpfCanvasSyncer(AbcCanvas abcVisual)
            : base(abcVisual, new Canvas())
        {
            this.nativeCanvas = (Canvas)this.nativeVisual;
        }
    }
}