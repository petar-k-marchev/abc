using Abc.Visuals;
using System.Windows.Controls;
using System.Windows.Media;

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

            this.nativeBorder.Background = Brushes.Black;
        }
    }
}