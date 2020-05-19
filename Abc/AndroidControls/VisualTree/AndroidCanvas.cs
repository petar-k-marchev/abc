using Abc.Visuals;
using Android.Graphics;
using Android.Views;
using AndroidControls.DataVisualization.Axes;

namespace AndroidControls.VisualTree
{
    internal class AndroidCanvas : AndroidVisualContainer, IAbcCanvas
    {
        private readonly SlotPanel slotPanel;

        internal AndroidCanvas()
            : base(new SlotPanel(AndroidNumericAxisControl.AndroidContext))
        {
            this.slotPanel = (SlotPanel)this.view;
        }

        internal override void ArrangeOverride(AbcArrangeContext context)
        {
            IAbcCanvas abcCanvas = this;

            foreach (IAbcVisual abcChild in abcCanvas.Children)
            {
                AbcContextualPropertyValue arrangeSlotPropertyValue = abcChild.GetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey);
                context.arrangeSlot = arrangeSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)arrangeSlotPropertyValue).value : abcCanvas.ArrangeSlot;

                var androidVisual = (AndroidVisual)abcChild;
                var slotView = androidVisual.view as IAbcAndroidSlotView;
                if (slotView != null)
                {
                    slotView.Slot = new Rect((int)context.arrangeSlot.x, (int)context.arrangeSlot.y, (int)context.arrangeSlot.Right(), (int)context.arrangeSlot.Bottom());
                }

                abcChild.Arrange(context);
            }

            base.ArrangeOverride(context);
        }
    }
}
