using Abc.Visuals;

namespace Abc.Controls
{
    internal abstract partial class AbcControl : IAbcControl
    {
        private bool isPaintPhase;

        protected abstract void PaintOverride(AbcContextBase context);

        public void Paint(AbcContextBase context)
        {
            this.isPaintPhase = true;
            this.PaintOverride(context);
            this.isPaintPhase = false;
        }
    }
}
