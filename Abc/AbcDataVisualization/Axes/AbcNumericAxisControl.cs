using Abc;
using Abc.Controls;
using Abc.Visuals;
using System;

namespace AbcDataVisualization
{
    internal class AbcNumericAxisControl : AbcControl
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

        private IAbcCanvas canvas;

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

        private IAbcCanvas Canvas
        {
            get
            {
                return this.canvas;
            }
            set
            {
                if (this.canvas != value)
                {
                    this.canvas = value;
                    this.SetRoot(value);
                }
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
                
                this.canvas.Children.Add(this.axisLine);
                this.canvas.Children.Add(this.firstLabel);
                this.canvas.Children.Add(this.lastLabel);
            }

            this.firstLabel.Text = "" + this.UserMin;
            this.lastLabel.Text = "" + this.UserMax;

            this.firstLabel.Measure(context);
            this.lastLabel.Measure(context);

            double desiredWidth = this.firstLabel.DesiredMeasure.width + this.lastLabel.DesiredMeasure.width;
            double desiredHeight = this.axisLineThickness + Math.Max(this.firstLabel.DesiredMeasure.height, this.lastLabel.DesiredMeasure.height);

            return new AbcSize(desiredWidth, desiredHeight);
        }

        protected override void ArrangeOverride(AbcArrangeContext context)
        {
            double axisX = context.arrangeSlot.x + (this.firstLabel.DesiredMeasure.width / 2);
            double axisRight = context.arrangeSlot.Right() - (this.lastLabel.DesiredMeasure.width / 2);
            AbcRect axisLineArrangeSlot = new AbcRect(axisX, 0, axisRight - axisX, this.axisLineThickness);
            this.axisLine.SetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = axisLineArrangeSlot });
            
            AbcRect firstLabelArrangeSlot = new AbcRect(0, axisLineArrangeSlot.Bottom(), this.firstLabel.DesiredMeasure.width, this.firstLabel.DesiredMeasure.height);
            this.firstLabel.SetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = firstLabelArrangeSlot });

            AbcRect lastLabelArrangeSlot = new AbcRect(axisLineArrangeSlot.Right() - (this.lastLabel.DesiredMeasure.width / 2), axisLineArrangeSlot.Bottom(), this.lastLabel.DesiredMeasure.width, this.lastLabel.DesiredMeasure.height);
            this.lastLabel.SetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = lastLabelArrangeSlot });

            this.Canvas.Arrange(context);
        }

        protected override void PaintOverride(AbcContextBase context)
        {
            this.Canvas.Paint(context);
        }

        protected override void OnVisualTreeChanged(AbcVisualTree oldVisualTree)
        {
            base.OnVisualTreeChanged(oldVisualTree);

            if (oldVisualTree != null)
            {
                this.canvas.Children.Clear();
                this.Canvas = null;
                this.axisLine = null;
                this.firstLabel = null;
                this.lastLabel = null;
            }

            if (this.VisualTree != null)
            {
                this.Canvas = (IAbcCanvas)this.VisualTree.CreateVisual(typeof(IAbcCanvas));
                this.Canvas.SetContextualPropertyValue(AbcControlContextualProperties.ControlPropertyKey, new AbcContextualPropertyValue.AbcObject { value = this });
            }
        }

        private void UpdateLabelsFontSize()
        {
            if (this.axisLine != null)
            {
                this.firstLabel.FontSize = this.FontSize;
                this.lastLabel.FontSize = this.FontSize;
            }
        }
    }
}
