using Abc;
using Abc.Visuals;

namespace WpfControls.WpfRenderingVisualTreeInternals
{
    internal class WpfCanvas : WpfVisualsContainer, IAbcCanvas
    {
        internal override void ArrangeOverride(AbcArrangeContext context)
        {
            IAbcCanvas abcCanvas = this;
            foreach (IAbcVisual child in abcCanvas.Children)
            {
                AbcContextualPropertyValue arrangeSlotPropertyValue = child.GetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey);
                context.arrangeSlot = arrangeSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)arrangeSlotPropertyValue).value : abcCanvas.ArrangeSlot;
                child.Arrange(context);
            }
        }
    }
}
