using Abc.Visuals;
using System.Windows;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfCanvas : WpfVisualsContainer, IAbcCanvas
    {
        private readonly SlotPanel slotPanel;

        internal WpfCanvas()
            : base(new SlotPanel())
        {
            this.slotPanel = (SlotPanel)this.uiElement;
        }

        internal override void ArrangeOverride(AbcArrangeContext context)
        {
            IAbcCanvas abcCanvas = this;

            foreach (IAbcVisual abcChild in abcCanvas.Children)
            {
                AbcContextualPropertyValue arrangeSlotPropertyValue = abcChild.GetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey);
                context.arrangeSlot = arrangeSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)arrangeSlotPropertyValue).value : abcCanvas.ArrangeSlot;

                WpfVisual wpfVisual = (WpfVisual)abcChild;
                FrameworkElement frameworkElement = (FrameworkElement)wpfVisual.uiElement;
                SlotPanel.SetSlot(frameworkElement, new Rect(context.arrangeSlot.x, context.arrangeSlot.y, context.arrangeSlot.size.width, context.arrangeSlot.size.height));

                abcChild.Arrange(context);
            }

            base.ArrangeOverride(context);
        }
    }
}