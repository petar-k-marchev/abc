using Abc.Primitives;

namespace Abc.Visuals
{
    internal class AbcStack : AbcVisualsContainer
    {
        protected override AbcSize MeasureOverride(AbcMeasureContext context)
        {
            if (this.children.Count == 0)
            {
                return new AbcSize();
            }

            AbcMeasureContext childContext = new AbcMeasureContext(context);
            childContext.availableSize = context.availableSize;
            double desiredWidth = 0;
            double desiredHeight = 0;

            foreach (AbcVisual child in this.children)
            {
                child.Measure(childContext);

                if (desiredWidth < child.DesiredMeasure.width)
                {
                    desiredWidth = child.DesiredMeasure.width;
                }
                desiredHeight += child.DesiredMeasure.height;

                childContext.availableSize.height -= child.DesiredMeasure.height;
                if (childContext.availableSize.height < 0)
                {
                    childContext.availableSize.height = 0;
                }
            }

            return new AbcSize(desiredWidth, desiredHeight);
        }
    }
}
