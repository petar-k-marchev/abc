using Abc.Visuals;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfRectangleSyncer : WpfVisualSyncer
    {
        private readonly Border nativeBorder;

        public WpfRectangleSyncer(AbcRectangle abcVisual)
            : base(abcVisual, new Border())
        {
            this.nativeBorder = (Border)this.nativeVisual;

            this.nativeBorder.Background = Brushes.Black;
        }
    }
}