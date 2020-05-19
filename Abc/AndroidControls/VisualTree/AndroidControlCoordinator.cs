using Abc;
using Abc.Controls;
using Android.Views;

namespace AndroidControls.VisualTree
{
    internal class AndroidControlCoordinator<T>
        where T : IAbcControl
    {
        internal readonly View nativeControl;
        internal readonly T abcControl;
        internal readonly AbcVisualTree visualTree;

        private ViewGroup nativeControlRoot;

        internal AndroidControlCoordinator(View nativeControl, T abcControl, AbcVisualTree visualTree)
        {
            this.nativeControl = nativeControl;
            this.abcControl = abcControl;
            this.visualTree = visualTree;
            this.abcControl.VisualTree = visualTree;

            this.abcControl.InvalidationRequest += this.AbcControl_InvalidationRequest;
        }

        internal ViewGroup NativeControlRoot
        {
            get
            {
                return this.nativeControlRoot;
            }
            set
            {
                if (this.nativeControlRoot != value)
                {
                    var androidVisual = this.abcControl.Root as AndroidVisual;

                    if (this.nativeControlRoot != null && androidVisual != null)
                    {
                        this.nativeControlRoot.RemoveView(androidVisual.view);
                    }

                    this.nativeControlRoot = value;

                    if (this.nativeControlRoot != null && androidVisual != null)
                    {
                        this.nativeControlRoot.AddView(androidVisual.view);
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
                    this.nativeControl.RequestLayout();
                    break;
                case InvalidationRequestFlag.Arrange:
                    this.nativeControl.RequestLayout();
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
