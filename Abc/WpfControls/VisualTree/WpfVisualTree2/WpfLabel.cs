using Abc.Visuals;
using System.Windows.Controls;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfLabel : WpfVisual, IAbcLabel
    {
        private readonly TextBlock textBlock;

        internal WpfLabel()
            : base(new TextBlock())
        {
            this.textBlock = (TextBlock)this.uiElement;
        }

        string IAbcLabel.Text
        {
            get { return this.textBlock.Text; }
            set { this.textBlock.Text = value; }
        }

        double IAbcLabel.FontSize
        {
            get
            {
                return this.textBlock.FontSize;
            }
            set
            {
                if (value > 0)
                {
                    this.textBlock.FontSize = value;
                }
                else
                {
                    this.textBlock.ClearValue(TextBlock.FontSizeProperty);
                }
            }
        }
    }
}