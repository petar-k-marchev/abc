using Abc.Visuals;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfRectangle : WpfVisual, IAbcRectangle
    {
        private readonly Border border;

        internal WpfRectangle()
            : base(new Border())
        {
            this.border = (Border)this.uiElement;

            this.border.Background = Brushes.Black;
        }
    }
}