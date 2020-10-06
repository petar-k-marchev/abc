using Abc.Visuals;
using System.Drawing;
using System.Windows.Forms;

namespace WFControls.VisualTree
{
    internal class WFCanvas : WFVisualsContainer, IAbcCanvas
    {
        internal WFCanvas()
            : base(new Panel())
        {
        }

        internal override void ArrangeOverride(AbcArrangeContext context)
        {
            IAbcCanvas abcCanvas = this;

            foreach (IAbcVisual abcChild in abcCanvas.Children)
            {
                AbcContextualPropertyValue arrangeSlotPropertyValue = abcChild.GetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey);
                context.arrangeSlot = arrangeSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)arrangeSlotPropertyValue).value : abcCanvas.ArrangeSlot;

                WFVisual wfVisual = (WFVisual)abcChild;
                Control visualControl = (Control)wfVisual.control;

                if (visualControl != null)
                {
                    visualControl.Bounds = new Rectangle((int)context.arrangeSlot.x, (int)context.arrangeSlot.y, (int)context.arrangeSlot.size.width, (int)context.arrangeSlot.size.height);
                }

                abcChild.Arrange(context);
            }

            base.ArrangeOverride(context);
        }
    }
}
