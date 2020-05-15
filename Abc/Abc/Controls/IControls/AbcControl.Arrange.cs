using Abc.Visuals;

namespace Abc.Controls
{
    internal abstract partial class AbcControl
    {
        private bool isArrangePhase;
        private AbcRect arrangeSlot;
        private bool isArrangeValid;

        public AbcRect ArrangeSlot
        {
            get
            {
                return this.arrangeSlot;
            }
        }

        protected abstract void ArrangeOverride(AbcArrangeContext context);

        internal void Arrange(AbcArrangeContext context)
        {
            if (this.isArrangeValid)
            {
                this.isArrangeValid = this.arrangeSlot == context.arrangeSlot;
            }

            if (this.isArrangeValid)
            {
                return;
            }

            this.isArrangePhase = true;
            this.arrangeSlot = context.arrangeSlot;
            this.ArrangeOverride(context);
            context.arrangeSlot = this.arrangeSlot;
            this.isArrangePhase = false;
            this.isArrangeValid = true;
        }

        internal void InvalidateArrange()
        {
            if (this.isArrangeValid)
            {
                this.isArrangeValid = false;
                this.InvalidationRequest(InvalidationRequestFlag.Arrange);
                this.InvalidatePaint();
            }
        }
    }
}
