using Abc.Visuals;
using System.Windows.Controls;

namespace WpfControls
{
    internal class WpfCanvasSyncer : WpfVisualSyncer
    {
        private readonly AbcCanvas abcCanvas;
        private readonly Canvas nativeCanvas;

        public WpfCanvasSyncer(AbcCanvas abcVisual)
            : base(abcVisual, new Canvas())
        {
            this.abcCanvas = abcVisual;
            this.nativeCanvas = (Canvas)this.nativeVisual;
        }
    }
}