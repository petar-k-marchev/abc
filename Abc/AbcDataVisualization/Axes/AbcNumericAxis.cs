using Abc.Primitives;
using Abc.Visuals;
using System;

namespace AbcDataVisualization
{
    internal class AbcNumericAxis : AbcCanvas
    {
        private int axisLineThickness = 4;

        private double dataMin;
        private double dataMax;

        private double userMin;
        private double userMax;
        private double userStep;

        private double actualMin;
        private double actualMax;

        private AbcRectangle axisLine;
        private AbcLabel firstLabel;
        private AbcLabel lastLabel;

        internal double UserMin
        {
            get
            {
                return this.userMin;
            }
            set
            {
                if (this.userMin == value)
                {
                    return;
                }

                this.userMin = value;
                this.AddFlag(AbcVisualFlag.AffectsLayoutAndMaybeMeasure);
            }
        }

        internal double UserMax
        {
            get
            {
                return this.userMax;
            }
            set
            {
                if (this.userMax == value)
                {
                    return;
                }

                this.userMax = value;
                this.AddFlag(AbcVisualFlag.AffectsLayoutAndMaybeMeasure);
            }
        }

        internal double UserStep
        {
            get
            {
                return this.userStep;
            }
            set
            {
                if (this.userStep == value)
                {
                    return;
                }

                this.userStep = value;
                this.AddFlag(AbcVisualFlag.AffectsLayoutOnly);
            }
        }

        protected override AbcSize MeasureOverride(AbcMeasureContext context)
        {
            if (this.axisLine == null)
            {
                this.axisLine = (AbcRectangle)this.VisualTree.CreateVisual(typeof(AbcRectangle));
                this.children.Add(this.axisLine);

                this.firstLabel = (AbcLabel)this.VisualTree.CreateVisual(typeof(AbcLabel));
                this.firstLabel.FontSize.Value = 30;
                this.children.Add(this.firstLabel);

                this.lastLabel = (AbcLabel)this.VisualTree.CreateVisual(typeof(AbcLabel));
                this.lastLabel.FontSize2.Value = 30;
                this.children.Add(this.lastLabel);
            }

            this.firstLabel.Text = "" + this.UserMin;
            this.lastLabel.Text = "" + this.UserMax;

            this.firstLabel.Measure(context);
            this.lastLabel.Measure(context);

            double desiredWidth = this.firstLabel.DesiredMeasure.width + this.lastLabel.DesiredMeasure.width;
            double desiredHeight = this.axisLineThickness + Math.Max(this.firstLabel.DesiredMeasure.height, this.lastLabel.DesiredMeasure.height);

            return new AbcSize(desiredWidth, desiredHeight);
        }

        protected override void LayoutOverride(AbcLayoutContext context)
        {
            double axisX = context.layoutSlot.x + (this.firstLabel.DesiredMeasure.width / 2);
            double axisRight = context.layoutSlot.Right() - (this.lastLabel.DesiredMeasure.width / 2);
            AbcRect axisLineLayoutSlot = new AbcRect(axisX, 0, axisRight - axisX, this.axisLineThickness);
            this.axisLine.SetContextualPropertyValue(LayoutSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = axisLineLayoutSlot });
            
            AbcRect firstLabelLayoutSlot = new AbcRect(0, axisLineLayoutSlot.Bottom(), this.firstLabel.DesiredMeasure.width, this.firstLabel.DesiredMeasure.height);
            this.firstLabel.SetContextualPropertyValue(LayoutSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = firstLabelLayoutSlot });

            AbcRect lastLabelLayoutSlot = new AbcRect(axisLineLayoutSlot.Right() - this.lastLabel.DesiredMeasure.width, axisLineLayoutSlot.Bottom(), this.lastLabel.DesiredMeasure.width, this.lastLabel.DesiredMeasure.height);
            this.lastLabel.SetContextualPropertyValue(LayoutSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = lastLabelLayoutSlot });

            base.LayoutOverride(context);
        }
    }
}
