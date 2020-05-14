using Abc;
using Abc.Visuals;
using WpfControls.WpfVisualTreeInternals;

namespace WpfControls
{
    internal class WpfVisualTree : AbcVisualTree
    {
        internal WpfVisualTree()
        {
            this.visualCreator[typeof(IAbcLabel)] = () => new WpfLabel();
            this.visualCreator[typeof(IAbcRectangle)] = () => new WpfRectangle();
            this.visualCreator[typeof(IAbcCanvas)] = () => new WpfCanvas();
        }
    }
}
