using Android.Content;
using Android.Graphics;
using Android.Views;

namespace AndroidControls.VisualTree
{
    internal class SlotPanel : ViewGroup
    {
        internal static readonly Rect invalidSlot = new Rect(int.MinValue, int.MinValue, 0, 0);
        public SlotPanel(Context context)
            : base(context)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            Rect bounds = new Rect(0, 0, r - l, b - t);

            for (int i = 0; i < this.ChildCount; i++)
            {
                var child = this.GetChildAt(i) as IAbcAndroidSlotView;
                if (child != null)
                {
                    var slot = child.Slot;
                    Rect actualSlot = slot == invalidSlot ? bounds : slot;
                    ((View)child).Layout(actualSlot.Left, actualSlot.Top, actualSlot.Right, actualSlot.Bottom);
                }
            }
        }
    }
}