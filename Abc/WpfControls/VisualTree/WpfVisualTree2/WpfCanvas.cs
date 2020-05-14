using Abc.Visuals;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfCanvas : WpfVisualsContainer, IAbcCanvas
    {
        private readonly Canvas canvas;

        internal WpfCanvas()
            : base(new Canvas())
        {
            this.canvas = (Canvas)this.uiElement;
        }

        internal override void ArrangeOverride(AbcArrangeContext context)
        {
            IAbcCanvas abcCanvas = this;

            foreach (IAbcVisual abcChild in abcCanvas.Children)
            {
                AbcContextualPropertyValue arrangeSlotPropertyValue = abcChild.GetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey);
                context.arrangeSlot = arrangeSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)arrangeSlotPropertyValue).value : abcCanvas.ArrangeSlot;

                WpfVisual wpfVisual = (WpfVisual)abcChild;
                FrameworkElement uiElement = (FrameworkElement)wpfVisual.uiElement;
                Canvas.SetLeft(uiElement, context.arrangeSlot.x);
                Canvas.SetTop(uiElement, context.arrangeSlot.y);
                uiElement.Width = context.arrangeSlot.size.width;
                uiElement.Height = context.arrangeSlot.size.height;

                abcChild.Arrange(context);
            }

            base.ArrangeOverride(context);
        }
    }
}