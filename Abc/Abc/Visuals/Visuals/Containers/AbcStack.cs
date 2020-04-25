using Abc.Primitives;

namespace Abc.Visuals
{
    internal class AbcStack : AbcVisualsContainer, IAbcStack
    {
        protected override AbcSize MeasureOverride(AbcMeasureContext context)
        {
            if (this.Children.Count == 0)
            {
                return new AbcSize();
            }

            double desiredWidth = 0;
            double desiredHeight = 0;

            foreach (AbcVisual child in this.Children)
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
