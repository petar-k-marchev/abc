using Abc;
using Abc.Visuals;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace WpfControls.WpfRenderingVisualTreeInternals
{
    internal class WpfLabel : WpfVisual, IAbcLabel
    {
        private FormattedText nativeFormattedText;
        private string text;
        private double fontSize;

        string IAbcLabel.Text
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
                    this.nativeFormattedText = null;
                    this.InvalidateMeasure();
                }
            }
        }

        double IAbcLabel.FontSize
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
                    this.OnFontSizeChanged();
                }
            }
        }

        internal override AbcSize MeasureOverride(AbcMeasureContext context)
        {
            if (this.TryEnsureFormattedText())
            {
                return new AbcSize(this.nativeFormattedText.Width, this.nativeFormattedText.Height);
            }
            else
            {
                return AbcSize.Zero;
            }
        }
        
        internal override void PaintOverride(AbcContextBase context)
        {
            if (!this.TryEnsureFormattedText())
            {
                return;
            }

            IAbcVisual abcVisual = this;
            AbcRect arrangeSlot = abcVisual.ArrangeSlot;

            object drawingContextObject;
            context.Bag.TryGetBagObject(WpfRenderingVisualTree.DrawingContextIdentifier, out drawingContextObject);
            DrawingContext drawingContext = (DrawingContext)drawingContextObject;

            Point position = new Point(arrangeSlot.x, arrangeSlot.y);
            drawingContext.DrawText(this.nativeFormattedText, position);
        }

        private static double GetEmSize(double fontSize)
        {
            double emSize = fontSize > 0 ? fontSize : SystemFonts.IconFontSize;
            return emSize;
        }

        private void OnFontSizeChanged()
        {
            if (this.nativeFormattedText == null)
            {
                return;
            }

            double emSize = GetEmSize(fontSize);
            this.nativeFormattedText.SetFontSize(emSize);
            this.InvalidateMeasure();
        }

        private bool TryEnsureFormattedText()
        {
            if (this.nativeFormattedText == null && this.text != null)
            {
                Typeface typeface = new Typeface(string.Empty);
                CultureInfo culture = CultureInfo.CurrentCulture;
                double fontSize = this.fontSize;
                double emSize = GetEmSize(fontSize);
                Brush foreground = Brushes.Black;
                this.nativeFormattedText = new FormattedText(this.text, culture, FlowDirection.LeftToRight, typeface, emSize, foreground, 1.25);
            }

            return this.nativeFormattedText != null;
        }
    }
}
