using Android.Content;
using Android.Graphics;
using Android.Views;

namespace AndroidControls.VisualTree
{
    public class AndroidSlotView : View, IAbcAndroidSlotView
    {
        private Rect slot;
        public AndroidSlotView(Context context)
            : base(context)
        {
        }

        Rect IAbcAndroidSlotView.Slot
        {
            get
            {
                return this.slot;
            }
            set
            {
                this.slot = value;
            }
        }
    }
}
