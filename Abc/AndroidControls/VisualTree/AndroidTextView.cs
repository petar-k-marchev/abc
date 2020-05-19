using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace AndroidControls.VisualTree
{
    internal class AndroidTextView : TextView, IAbcAndroidSlotView
    {
        private Rect slot;
        public AndroidTextView(Context context)
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
