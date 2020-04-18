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

            double desiredWidth = 0;
            double desiredHeight = 0;

            foreach (AbcVisual child in this.children)
            {
                child.Measure(context);

                if (desiredWidth < child.DesiredMeasure.width)
                {
                    desiredWidth = child.DesiredMeasure.width;
                }
                desiredHeight += child.DesiredMeasure.height;

                context.availableSize.height -= child.DesiredMeasure.height;
                if (context.availableSize.height < 0)
                {
                    context.availableSize.height = 0;
                }
            }

            return new AbcSize(desiredWidth, desiredHeight);
        }
    }
}
