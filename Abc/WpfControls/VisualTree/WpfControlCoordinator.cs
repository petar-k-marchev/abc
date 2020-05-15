using Abc;
using Abc.Controls;
using System.Windows.Controls;
using WpfControls.WpfVisualTreeInternals;

namespace WpfControls.DataVisualization
{
    internal class WpfControlCoordinator<T>
        where T : IAbcControl
    {
        internal readonly Control nativeControl;
        internal readonly T abcControl;
        internal readonly AbcVisualTree visualTree;

        private Panel nativeControlRoot;

        internal WpfControlCoordinator(Control nativeControl, T abcControl, AbcVisualTree visualTree)
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
                    WpfVisual wpfVisual = this.abcControl.Root as WpfVisual;

                    if (this.nativeControlRoot != null && wpfVisual != null)
                    {
                        this.nativeControlRoot.Children.Remove(wpfVisual.uiElement);
                    }

                    this.nativeControlRoot = value;

                    if (this.nativeControlRoot != null && wpfVisual != null)
                    {
                        this.nativeControlRoot.Children.Add(wpfVisual.uiElement);
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
                    this.nativeControl.InvalidateMeasure();
                    break;
                case InvalidationRequestFlag.Arrange:
                    this.nativeControl.InvalidateArrange();
                    break;
                case InvalidationRequestFlag.Paint:
                    this.nativeControl.InvalidateVisual();
                    break;
                default:
                    break;
            }
        }
    }
}
