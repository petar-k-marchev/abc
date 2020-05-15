using Abc;
using Abc.Controls;
using Abc.Visuals;
using System;
using System.Collections.Generic;

namespace AbcDataVisualization
{
    internal class AbcNumericAxisControl : AbcControl
    {
        private int axisLineThickness = 2;
        private int axisTickThickness = 2;
        private int axisTickLength = 5;

        private double userMin;
        private double userMax;
        private double userStep;
        private double fontSize;

        private IAbcRectangle axisLine;
        private List<AxisTickInfo> tickInfos;
        private AbcVisualsPool<IAbcRectangle> ticksPool;
        private List<AxisLabelInfo> labelInfos;
        private AbcVisualsPool<IAbcLabel> labelsPool;

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
                this.ResetAxisRange();
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
                this.ResetAxisRange();
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
                this.ResetAxisRange();
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
                this.canvas.Children.Add(this.axisLine);
            }

            this.EnsureLabelInfos();
            AbcSize labelsDesiredSize = this.MeasureLabels(context);

            double desiredWidth = labelsDesiredSize.width;
            double desiredHeight = this.axisLineThickness + this.axisTickThickness + labelsDesiredSize.height;

            return new AbcSize(desiredWidth, desiredHeight);
        }

        protected override void ArrangeOverride(AbcArrangeContext context)
        {
            double firstLabelWidth = 0;
            double lastLabelWidth = 0;

            if (this.labelInfos.Count > 0)
            {
                firstLabelWidth = this.labelsPool.GetOrCreate(0).DesiredMeasure.width;
                lastLabelWidth = this.labelsPool.GetOrCreate(this.labelInfos.Count - 1).DesiredMeasure.width;
            }

            double right = context.arrangeSlot.Right();
            double axisLineX = context.arrangeSlot.x + (firstLabelWidth / 2);
            double axisLineRight = right - (lastLabelWidth / 2);
            AbcRect axisLineArrangeSlot = new AbcRect(axisLineX, 0, axisLineRight - axisLineX, this.axisLineThickness);
            this.axisLine.SetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = axisLineArrangeSlot });

            this.EnsureTickInfos();
            this.ArrangeTicks(axisLineX, axisLineRight, axisLineArrangeSlot.Bottom());

            this.ArrangeLabels(axisLineX, axisLineRight, axisLineArrangeSlot.Bottom() + this.axisTickLength);

            this.Canvas.Arrange(context);
        }

        protected override void PaintOverride(AbcArrangeContext context)
        {
            this.Canvas.Paint(context);
        }

        protected override void OnVisualTreeChanged(AbcVisualTree oldVisualTree)
        {
            base.OnVisualTreeChanged(oldVisualTree);

            if (oldVisualTree != null)
            {
                this.ticksPool.Clear();
                this.ticksPool = null;
                this.labelsPool.Clear();
                this.labelsPool = null;
                this.canvas.Children.Clear();
                this.Canvas = null;
                this.axisLine = null;
            }

            if (this.VisualTree != null)
            {
                this.Canvas = (IAbcCanvas)this.VisualTree.CreateVisual(typeof(IAbcCanvas));
                this.Canvas.SetContextualPropertyValue(AbcControlContextualProperties.ControlPropertyKey, new AbcContextualPropertyValue.AbcObject { value = this });
                this.ticksPool = new AbcVisualsPool<IAbcRectangle>(this.Canvas, this.CreateTick);
                this.labelsPool = new AbcVisualsPool<IAbcLabel>(this.Canvas, this.CreateLabel);
            }
        }

        private void ResetAxisRange()
        {
            this.tickInfos?.Clear();
            this.labelInfos?.Clear();
            this.InvalidateMeasure();
        }

        private void UpdateLabelsFontSize()
        {
            if (this.labelsPool != null)
            {
                foreach (IAbcLabel labelVisual in this.labelsPool.GetAll())
                {
                    labelVisual.FontSize = this.fontSize;
                }
            }
        }

        private void EnsureTickInfos()
        {
            if (this.tickInfos != null && this.tickInfos.Count != 0)
            {
                return;
            }

            if (this.tickInfos == null)
            {
                this.tickInfos = new List<AxisTickInfo>();
            }

            double range = this.userMax - this.userMin;
            bool isRangeValid = AbcMath.IsValidPositiveDouble(this.userStep) &&
                AbcMath.IsValidPositiveDouble(range);

            if (!isRangeValid)
            {
                return;
            }

            for (int i = 0; ; i++)
            {
                double currValue = this.userMin + (i * this.userStep);
                if (this.userMax < currValue)
                {
                    break;
                }

                double relativePosition = this.CalculateRelativePosition(currValue);
                this.tickInfos.Add(new AxisTickInfo { relativePosition = relativePosition });
            }
        }

        private void ArrangeTicks(double axisLineX, double axisLineRight, double y)
        {
            double axisLineLength =axisLineRight - axisLineX;
            double halfTickThickness = this.axisTickThickness / 2;
            int count = this.tickInfos.Count;

            for (int i = 0; i < count; i++)
            {
                AxisTickInfo tickInfo = this.tickInfos[i];
                IAbcRectangle tickVisual = this.ticksPool.GetOrCreate(i);
                double x = axisLineX + (tickInfo.relativePosition * axisLineLength);
                AbcRect tickSlot = new AbcRect(x - halfTickThickness, y, this.axisTickThickness, this.axisTickLength);
                tickVisual.SetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = tickSlot });
            }

            this.ticksPool.HideAfter(count);
        }

        private void EnsureLabelInfos()
        {
            if (this.labelInfos != null && this.labelInfos.Count != 0)
            {
                return;
            }

            if (this.labelInfos == null)
            {
                this.labelInfos = new List<AxisLabelInfo>();
            }

            double range = this.userMax - this.userMin;
            bool isRangeValid = AbcMath.IsValidPositiveDouble(this.userStep) &&
                AbcMath.IsValidPositiveDouble(range);

            if (!isRangeValid)
            {
                return;
            }

            for (int i = 0; ; i++)
            {
                double currValue = this.userMin + (i * this.userStep);
                if (this.userMax < currValue)
                {
                    break;
                }

                double relativePosition = this.CalculateRelativePosition(currValue);
                string labelContent = "" + currValue;
                this.labelInfos.Add(new AxisLabelInfo { relativePosition = relativePosition, value = currValue, labelContent = labelContent });
            }
        }

        private AbcSize MeasureLabels(AbcMeasureContext context)
        {
            int count = this.labelInfos.Count;

            for (int i = 0; i < count; i++)
            {
                AxisLabelInfo labelInfo = this.labelInfos[i];
                IAbcLabel labelVisual = this.labelsPool.GetOrCreate(i);
                labelVisual.Text = labelInfo.labelContent;
                labelVisual.Measure(context);
            }

            this.labelsPool.HideAfter(count);

            if (count > 0)
            {
                IAbcLabel firstLabel = this.labelsPool.GetOrCreate(0);
                IAbcLabel lastLabel = this.labelsPool.GetOrCreate(count - 1);
                double desiredWidth = firstLabel.DesiredMeasure.width + lastLabel.DesiredMeasure.width;
                double desiredHeight = Math.Max(firstLabel.DesiredMeasure.height, lastLabel.DesiredMeasure.height);
                return new AbcSize(desiredWidth, desiredHeight);
            }
            else
            {
                return AbcSize.Zero;
            }
        }

        private void ArrangeLabels(double axisLineX, double axisLineRight, double y)
        {
            double axisLineLength = axisLineRight - axisLineX;
            int count = this.labelInfos.Count;

            for (int i = 0; i < count; i++)
            {
                AxisLabelInfo labelInfo = this.labelInfos[i];
                IAbcLabel labelVisual = this.labelsPool.GetOrCreate(i);
                double x = axisLineX + (labelInfo.relativePosition * axisLineLength);
                AbcRect tickSlot = new AbcRect(x - (labelVisual.DesiredMeasure.width / 2), y, labelVisual.DesiredMeasure.width, labelVisual.DesiredMeasure.height);
                labelVisual.SetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey, new AbcContextualPropertyValue.AbcRect { value = tickSlot });
            }

            this.labelsPool.HideAfter(count);
        }

        private double CalculateRelativePosition(double value)
        {
            double relativeValue = (value - this.userMin) / (this.userMax - this.userMin);
            return relativeValue;
        }

        private IAbcRectangle CreateTick()
        {
            IAbcRectangle visual = (IAbcRectangle)this.VisualTree.CreateVisual(typeof(IAbcRectangle));
            return visual;
        }

        private IAbcLabel CreateLabel()
        {
            IAbcLabel visual = (IAbcLabel)this.VisualTree.CreateVisual(typeof(IAbcLabel));
            visual.FontSize = this.fontSize;
            return visual;
        }
    }
}
