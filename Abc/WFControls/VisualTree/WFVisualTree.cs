using Abc;
using Abc.Visuals;

namespace WFControls.VisualTree
{
    internal class WFVisualTree : AbcVisualTree
    {
        internal WFVisualTree()
        {
            this.visualCreator[typeof(IAbcLabel)] = () => new WFLabel();
            this.visualCreator[typeof(IAbcRectangle)] = () => new WFRectangle();
            this.visualCreator[typeof(IAbcCanvas)] = () => new WFCanvas();
        }
    }
}
