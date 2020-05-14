using Abc.Primitives;
using Abc.Visuals;

namespace WpfControls
{
    internal class WpfDrawCanvasVisual : WpfDrawVisualsContainerVisual, IAbcCanvas
    {
        internal override AbcSize MeasureOverride(AbcMeasureContext context)
        {
            return new AbcSize(0, 0);
        }

        internal override void ArrangeOverride(AbcArrangeContext context)
        {
            IAbcVisualsContainer abcVisualsContainer = this;
            foreach (IAbcVisual child in abcVisualsContainer.Children)
            {
                AbcContextualPropertyValue arrangeSlotPropertyValue = child.GetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey);
                context.arrangeSlot = arrangeSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)arrangeSlotPropertyValue).value : AbcRect.Empty;
                child.Arrange(context);
            }
        }
    }
}
