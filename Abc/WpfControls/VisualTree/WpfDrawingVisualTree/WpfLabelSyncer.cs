using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace WpfControls.WpfDrawingVisualTreeInternals
{
    internal class WpfLabelSyncer : WpfDrawCommandSyncer
    {
        private FormattedText nativeFormattedText;

        public WpfLabelSyncer(AbcVisual abcVisual)
            : base(abcVisual, hasContextualListeners: true)
        {
        }

        internal override AbcSize Measure(AbcMeasureContext context)
        {
            this.EnsureFormattedText();
            return new AbcSize(this.nativeFormattedText.Width, this.nativeFormattedText.Height);
        }

        internal override void StartSync()
        {
            base.StartSync();

            AbcLabel abcLabel = (AbcLabel)this.abcVisual;
            abcLabel.TextChanged += this.AbcLabel_TextChanged;
            abcLabel.FontSizeChanged += this.AbcLabel_FontSizeChanged;
        }

        internal override void StopSync()
        {
            base.StopSync();

            AbcLabel abcLabel = (AbcLabel)this.abcVisual;
            abcLabel.TextChanged -= this.AbcLabel_TextChanged;
            abcLabel.FontSizeChanged -= this.AbcLabel_FontSizeChanged;

            this.nativeFormattedText = null;
        }

        internal void OnRender(DrawingContext dc)
        {
            this.EnsureFormattedText();

            AbcContextualPropertyValue layoutSlotPropertyValue = this.abcVisual.GetContextualPropertyValue(AbcCanvasContextualProperties.LayoutSlotPropertyKey);
            AbcRect layoutSlot = layoutSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)layoutSlotPropertyValue).value : AbcRect.Empty;

            Point position = new Point(layoutSlot.x, layoutSlot.y);
            dc.DrawText(this.nativeFormattedText, position);
        }

        private void AbcLabel_TextChanged(object sender, System.EventArgs e)
        {
            this.nativeFormattedText = null;
        }

        private void AbcLabel_FontSizeChanged(object sender, EventArgs e)
        {
            this.nativeFormattedText = null;
        }

        private void FontSize2_Changed(object sender, AbcProperty<double>.AbcPropertyChangedEventArgs e)
        {
            this.nativeFormattedText = null;
        }

        private void EnsureFormattedText()
        {
            if (this.nativeFormattedText == null)
            {
                AbcLabel abcLabel = (AbcLabel)this.abcVisual;
                Typeface typeface = new Typeface(string.Empty);
                CultureInfo culture = CultureInfo.CurrentCulture;
                double fontSize = abcLabel.FontSize;
                double emSize = fontSize > 0 ? fontSize : SystemFonts.IconFontSize;
                Brush foreground = Brushes.Black;
                this.nativeFormattedText = new FormattedText(abcLabel.Text, culture, FlowDirection.LeftToRight, typeface, emSize, foreground, 1.25);
            }
        }
    }
}
