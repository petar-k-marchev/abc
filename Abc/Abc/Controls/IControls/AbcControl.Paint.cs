using Abc.Visuals;

namespace Abc.Controls
{
    internal abstract partial class AbcControl
    {
        private bool isPaintPhase;
        private bool isPaintValid;

        protected abstract void PaintOverride(AbcContextBase context);

        internal void Paint(AbcContextBase context)
        {
            this.isPaintPhase = true;
            this.PaintOverride(context);
            this.isPaintPhase = false;
            this.isPaintValid = true;
        }

        internal void InvalidatePaint()
        {
            if (this.isPaintValid)
            {
                this.isPaintValid = false;
            }
        }
    }
}
