using Abc.Visuals;

namespace Abc.Controls
{
    internal abstract partial class AbcControl
    {
        AbcVisualTree IAbcControl.VisualTree
        {
            get
            {
                return this.VisualTree;
            }
            set
            {
                this.VisualTree = value;
            }
        }

        IAbcVisual IAbcControl.Root
        {
            get
            {
                return this.Root;
            }
        }

        void IAbcControl.OnRootMeasureInvalidated()
        {
            this.InvalidateMeasure();
        }
    }
}
