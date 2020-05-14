using Abc;
using Abc.Visuals;
using WpfControls.WpfRenderingVisualTreeInternals;

namespace WpfControls
{
    internal class WpfRenderingVisualTree : AbcVisualTree
    {
        internal static readonly string DrawingContextIdentifier = "System.Windows.Media.DrawingContext";

        internal WpfRenderingVisualTree()
        {
            this.visualCreator[typeof(IAbcLabel)] = () => new WpfLabel();
            this.visualCreator[typeof(IAbcCanvas)] = () => new WpfCanvas();
            this.visualCreator[typeof(IAbcRectangle)] = () => new WpfRectangle();
        }
    }
}
