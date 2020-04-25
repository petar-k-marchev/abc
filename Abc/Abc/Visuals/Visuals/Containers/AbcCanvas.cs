using Abc.Primitives;

namespace Abc.Visuals
{
    internal class AbcCanvas : AbcVisualsContainer, IAbcCanvas
    {
        protected override void LayoutOverride(AbcLayoutContext context)
        {
            base.LayoutOverride(context);
            
            AbcRect contextLayoutSlot = context.layoutSlot;

            foreach (AbcVisual child in this.Children)
            {
                AbcContextualPropertyValue layoutSlotPropertyValue = child.GetContextualPropertyValue(AbcCanvasContextualProperties.LayoutSlotPropertyKey);
                context.layoutSlot = layoutSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)layoutSlotPropertyValue).value : AbcRect.Empty;
                child.Layout(context);
            }

            context.layoutSlot = contextLayoutSlot;
        }
    }
}
