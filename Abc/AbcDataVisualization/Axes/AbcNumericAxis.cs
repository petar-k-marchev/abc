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
        private double fontSize;

        private IAbcRectangle axisLine;
        private IAbcLabel firstLabel;
        private IAbcLabel lastLabel;

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

        internal double FontSize
        {
            get
            {
                return this.fontSize;
            }
            set
            {
                if (this.fontSize == value)
                {
                    return;
                }

                this.fontSize = value;
                this.UpdateLabelsFontSize();
            }
        }

        protected override AbcSize MeasureOverride(AbcMeasureContext context)
        {
            if (this.axisLine == null)
            {
                this.axisLine = (IAbcRectangle)this.VisualTree.CreateVisual(typeof(IAbcRectangle));
                this.firstLabel = (IAbcLabel)this.VisualTree.CreateVisual(typeof(IAbcLabel));
                this.lastLabel = (IAbcLabel)this.VisualTree.CreateVisual(typeof(IAbcLabel));
                
                this.UpdateLabelsFontSize();
                
                this.Children.Add(this.axisLine);
                this.Children.Add(this.firstLabel);
                this.Children.Add(this.lastLabel);
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
            this.axisLine.SetContextualPropertyValue(AbcCanvasContextualProperties.LayoutSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = axisLineLayoutSlot });
            
            AbcRect firstLabelLayoutSlot = new AbcRect(0, axisLineLayoutSlot.Bottom(), this.firstLabel.DesiredMeasure.width, this.firstLabel.DesiredMeasure.height);
            this.firstLabel.SetContextualPropertyValue(AbcCanvasContextualProperties.LayoutSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = firstLabelLayoutSlot });

            AbcRect lastLabelLayoutSlot = new AbcRect(axisLineLayoutSlot.Right() - this.lastLabel.DesiredMeasure.width, axisLineLayoutSlot.Bottom(), this.lastLabel.DesiredMeasure.width, this.lastLabel.DesiredMeasure.height);
            this.lastLabel.SetContextualPropertyValue(AbcCanvasContextualProperties.LayoutSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = lastLabelLayoutSlot });

            base.LayoutOverride(context);
        }

        private void UpdateLabelsFontSize()
        {
            if (this.axisLine != null)
            {
                this.firstLabel.FontSize = this.FontSize;
            }
        }
    }
}
