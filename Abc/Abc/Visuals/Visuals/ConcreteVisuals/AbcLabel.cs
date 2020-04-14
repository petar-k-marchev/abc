using System;

namespace Abc.Visuals
{
    internal class AbcLabel : AbcVisual
    {
        private string text;

        internal event EventHandler TextChanged;

        internal string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.AddFlag(AbcVisualFlag.AffectsLayoutAndMaybeMeasure);
                    this.TextChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
