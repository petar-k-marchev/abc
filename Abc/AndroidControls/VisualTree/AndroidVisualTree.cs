using Abc;
using Abc.Visuals;

namespace AndroidControls.VisualTree
{
    internal class AndroidVisualTree : AbcVisualTree
    {
        internal AndroidVisualTree()
        {
            this.visualCreator[typeof(IAbcLabel)] = () => new AndroidLabel();
            this.visualCreator[typeof(IAbcRectangle)] = () => new AndroidRectangle();
            this.visualCreator[typeof(IAbcCanvas)] = () => new AndroidCanvas();
        }
    }
}
