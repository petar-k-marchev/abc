using Abc;
using Abc.Controls;
using System.Windows.Forms;

namespace WFControls.VisualTree
{
    internal class WFControlCoordinator<T>
        where T : IAbcControl
    {
        internal readonly Control nativeControl;
        internal readonly T abcControl;
        internal readonly AbcVisualTree visualTree;

        private Panel nativeControlRoot;

        internal WFControlCoordinator(Control nativeControl, T abcControl, AbcVisualTree visualTree)
        {
            this.nativeControl = nativeControl;
            this.abcControl = abcControl;
            this.visualTree = visualTree;
            this.abcControl.VisualTree = visualTree;

            this.abcControl.InvalidationRequest += this.AbcControl_InvalidationRequest;
        }

        internal Panel NativeControlRoot
        {
            get
            {
                return this.nativeControlRoot;
            }
            set
            { 
                if (this.nativeControlRoot != value)
                {
                    WFVisual wfVisual = this.abcControl.Root as WFVisual;

                    if (this.nativeControlRoot != null && wfVisual != null)
                    {
                        this.nativeControlRoot.Controls.Remove(wfVisual.control);
                    }

                    this.nativeControlRoot = value;

                    if (this.nativeControlRoot != null && wfVisual != null)
                    {
                        this.nativeControlRoot.Controls.Add(wfVisual.control);
                    }
                }
            }
        }

        private void AbcControl_InvalidationRequest(object sender, InvalidationRequestEventArgs args)
        {
            switch (args.flag)
            {
                case InvalidationRequestFlag.None:
                    break;
                case InvalidationRequestFlag.Measure:
                    this.nativeControl.Invalidate();
                    break;
                case InvalidationRequestFlag.Arrange:
                    this.nativeControl.Invalidate();
                    break;
                case InvalidationRequestFlag.Paint:
                    this.nativeControl.Invalidate();
                    break;
                default:
                    break;
            }
        }
    }
}
