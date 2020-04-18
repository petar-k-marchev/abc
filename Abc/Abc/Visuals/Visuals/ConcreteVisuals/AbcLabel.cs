using System;

namespace Abc.Visuals
{
    internal class AbcLabel : AbcVisual
    {
        private string text;

        public AbcLabel()
        {
            this.FontSize2 = new AbcProperty<double>(this, AbcVisualFlag.AffectsMeasureAndLayout);
        }

        internal event EventHandler TextChanged;

        internal AbcProperty.DoubleWithDefault FontSize { get; } = new AbcProperty.DoubleWithDefault(double.NaN);

        internal AbcProperty<double> FontSize2 { get; }

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
