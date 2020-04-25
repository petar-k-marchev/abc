using System;

namespace Abc.Visuals
{
    internal class AbcLabel : AbcVisual, IAbcLabel
    {
        private string text;
        private double fontSize = -1;

        internal event EventHandler TextChanged;
        internal event EventHandler FontSizeChanged;

        public string Text
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
                    this.AddFlag(AbcVisualFlag.AffectsArrangeAndMaybeMeasure);
                    this.TextChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public double FontSize
        {
            get
            {
                return this.fontSize;
            }
            set
            {
                if (this.fontSize != value)
                {
                    this.fontSize = value;
                    this.AddFlag(AbcVisualFlag.AffectsMeasureAndArrange);
                    this.FontSizeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
