using Abc.Visuals;

namespace Abc.Controls
{
    internal abstract partial class AbcControl : IAbcControl
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

        public void Measure(AbcMeasureContext context)
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
    }
}
