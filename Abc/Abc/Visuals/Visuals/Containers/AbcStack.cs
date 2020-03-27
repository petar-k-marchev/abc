namespace Abc.Visuals
{
    internal class AbcStack : AbcVisualsContainer
    {
        protected override AbcSize CalculateDesiredSizeOverride(AbcMeasureContext context)
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
                child.CalculateDesiredSize(childContext);

                if (desiredWidth < child.DesiredSize.width)
                {
                    desiredWidth = child.DesiredSize.width;
                }
                desiredHeight += child.DesiredSize.height;

                childContext.availableSize.height -= child.DesiredSize.height;
                if (childContext.availableSize.height < 0)
                {
                    childContext.availableSize.height = 0;
                }
            }

            return new AbcSize(desiredWidth, desiredHeight);
        }
    }
}
