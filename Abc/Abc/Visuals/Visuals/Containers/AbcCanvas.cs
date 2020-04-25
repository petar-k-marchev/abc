using Abc.Primitives;

namespace Abc.Visuals
{
    internal class AbcCanvas : AbcVisualsContainer, IAbcCanvas
    {
        protected override void ArrangeOverride(AbcArrangeContext context)
        {
            base.ArrangeOverride(context);
            
            foreach (AbcVisual child in this.Children)
            {
                AbcContextualPropertyValue arrangeSlotPropertyValue = child.GetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey);
                context.arrangeSlot = arrangeSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)arrangeSlotPropertyValue).value : AbcRect.Empty;
                child.Arrange(context);
            }
        }
    }
}
