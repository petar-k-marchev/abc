using Abc.Primitives;

namespace Abc.Visuals
{
    internal class AbcCanvas : AbcVisualsContainer
    {
        internal static readonly AbcContextualPropertyKey LayoutSlotPropertyKey = new AbcContextualPropertyKey();

        protected override void LayoutOverride(AbcLayoutContext context)
        {
            base.LayoutOverride(context);
            
            AbcRect contextLayoutSlot = context.layoutSlot;

            foreach (AbcVisual child in this.children)
            {
                AbcContextualPropertyValue layoutSlotPropertyValue = child.GetContextualPropertyValue(LayoutSlotPropertyKey);
                context.layoutSlot = layoutSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)layoutSlotPropertyValue).value : AbcRect.Empty;
                child.Layout(context);
            }

            context.layoutSlot = contextLayoutSlot;
        }
    }
}
