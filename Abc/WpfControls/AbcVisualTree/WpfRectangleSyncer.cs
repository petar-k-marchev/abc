using Abc.Visuals;
using System.Windows.Controls;

namespace WpfControls
{
    internal class WpfRectangleSyncer : WpfVisualSyncer
    {
        private readonly AbcRectangle abcRectangle;
        private readonly Border nativeBorder;

        public WpfRectangleSyncer(AbcRectangle abcVisual)
            : base(abcVisual, new Border())
        {
            this.abcRectangle = abcVisual;
            this.nativeBorder = (Border)this.nativeVisual;
        }
    }
}