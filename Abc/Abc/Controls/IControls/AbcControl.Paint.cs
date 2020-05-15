using Abc.Visuals;

namespace Abc.Controls
{
    internal abstract partial class AbcControl
    {
        private bool isPaintPhase;
        private bool isPaintValid;

        protected abstract void PaintOverride(AbcArrangeContext context);

        internal void Paint(AbcArrangeContext context)
        {
            if (!this.isArrangeValid)
            {
                this.Arrange(context);
            }

            this.isPaintPhase = true;
            AbcRect arrangeSlot = context.arrangeSlot;
            this.PaintOverride(context);
            context.arrangeSlot = arrangeSlot;
            this.isPaintPhase = false;
            this.isPaintValid = true;
        }

        internal void InvalidatePaint()
        {
            if (this.isPaintValid)
            {
                this.isPaintValid = false;
                this.InvalidationRequest(InvalidationRequestFlag.Paint);
            }
        }
    }
}
