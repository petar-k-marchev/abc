using Abc.Visuals;
using Android.Graphics;
using AndroidControls.DataVisualization.Axes;

namespace AndroidControls.VisualTree
{
    internal class AndroidRectangle : AndroidVisual, IAbcRectangle
    {
        private readonly AndroidSlotView border;

        internal AndroidRectangle()
            : base(new AndroidSlotView(AndroidNumericAxisControl.AndroidContext))
        {
            this.border = (AndroidSlotView)this.view;
            this.border.SetBackgroundColor(Color.Red);
        }
    }
}