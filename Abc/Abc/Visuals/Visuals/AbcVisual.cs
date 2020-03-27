using System.Collections.Generic;

namespace Abc.Visuals
{
    internal abstract class AbcVisual
    {
        private AbcVisualTree visualTree;
        private Dictionary<int, AbcContextualPropertyValue> contextualProperties;
        private AbcSize desiredSize;
        private bool isDesiredSizeValid;

        internal AbcVisualTree VisualTree
        {
            get
            {
                return this.visualTree;
            }
        }

        internal AbcSize DesiredSize
        {
            get
            {
                return this.desiredSize;
            }
        }

        protected virtual AbcSize CalculateDesiredSizeOverride(AbcMeasureContext context)
        {
            AbcSize size = this.VisualTree.CalculateDesiredSize(context);
            return size;
        }

        internal void CalculateDesiredSize(double availableWidth, double availableHeight)
        {
            this.CalculateDesiredSize(new AbcSize(availableWidth, availableHeight));
        }

        internal void CalculateDesiredSize(AbcSize availableSize)
        {
            this.CalculateDesiredSize(new AbcMeasureContext(availableSize));
        }

        internal void CalculateDesiredSize(AbcMeasureContext context)
        {
            this.desiredSize = this.CalculateDesiredSizeOverride(context);
            this.isDesiredSizeValid = true;
        }

        internal AbcContextualPropertyValue GetContextualPropertyValue(AbcContextualPropertyKey propertyKey)
        {
            AbcContextualPropertyValue propertyValue = null;

            this.contextualProperties?.TryGetValue(propertyKey.key, out propertyValue);
            // if not present - perhaps some get-on-demand default value (propertyKey.GetDefaultPropertyValue())

            return propertyValue;
        }

        internal void SetContextualPropertyValue(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue propertyValue)
        {
            if (this.contextualProperties == null)
            {
                this.contextualProperties = new Dictionary<int, AbcContextualPropertyValue>();
            }

            this.contextualProperties[propertyKey.key] = propertyValue;

            // property changed notifications ?
        }

        internal void AddFlag(AbcVisualFlag flag)
        {
            if (flag == AbcVisualFlag.None)
            {
                return;
            }

            if (flag == AbcVisualFlag.AffectsMeasureAndLayout)
            {
                if (this.isDesiredSizeValid)
                {
                    //this.Send Measure Request
                    //this.Send Layout Request
                }
            }

            if (flag == AbcVisualFlag.AffectsLayout)
            {
                //this.Send Measure Request
            }
        }
    }
}
