using System;

namespace Abc.Visuals
{
    internal class AbcLabel : AbcVisual
    {
        private string text;

        public AbcLabel()
        {
            this.FontSize = new AbcProperty.DoubleWithDefault(double.NaN);
        }

        internal event EventHandler TextChanged;

        internal AbcProperty.DoubleWithDefault FontSize { get; }

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
