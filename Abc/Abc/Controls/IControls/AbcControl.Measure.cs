using Abc.Visuals;
using System;

namespace Abc.Controls
{
    internal abstract partial class AbcControl
    {
        private bool isMeasurePhase;
        private AbcSize desiredMeasure;
        private bool isMeasureValid;

        public AbcSize DesiredMeasure
        {
            get
            {
                return this.desiredMeasure;
            }
        }

        protected abstract AbcSize MeasureOverride(AbcMeasureContext context);

        internal void Measure(AbcMeasureContext context)
        {
            if (this.isMeasureValid)
            {
                return;
            }

            this.isMeasurePhase = true;
            this.desiredMeasure = this.MeasureOverride(context);
            this.isMeasurePhase = false;
            this.isMeasureValid = true;
        }

        internal void InvalidateMeasure()
        {
            if (this.isMeasureValid)
            {
                this.isMeasureValid = false;
                this.InvalidationRequest(InvalidationRequestFlag.Measure);
                this.InvalidateArrange();
            }
        }

        private void InvalidationRequest(InvalidationRequestFlag flag)
        {
            this.invalidationRequest?.Invoke(this, new InvalidationRequestEventArgs(flag));
        }
    }
}
