using Abc.Visuals;
using System;
using System.Windows.Controls;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfLabelSyncer : WpfVisualSyncer
    {
        private readonly TextBlock nativeTextBlock;

        public WpfLabelSyncer(AbcLabel abcVisual)
            : base(abcVisual, new TextBlock())
        {
            this.nativeTextBlock = (TextBlock)this.nativeVisual;
        }

        internal override void StartSync()
        {
            base.StartSync();

            AbcLabel abcLabel = (AbcLabel)this.abcVisual;

            this.nativeTextBlock.Text = abcLabel.Text;
            this.UpdateFontSize();

            abcLabel.TextChanged += this.AbcLabel_TextChanged;
            abcLabel.FontSizeChanged += this.AbcLabel_FontSizeChanged;
        }

        internal override void StopSync()
        {
            base.StopSync();

            AbcLabel abcLabel = (AbcLabel)this.abcVisual;

            abcLabel.TextChanged -= this.AbcLabel_TextChanged;
            abcLabel.FontSizeChanged -= this.AbcLabel_FontSizeChanged;
        }

        private void AbcLabel_TextChanged(object sender, System.EventArgs e)
        {
            AbcLabel abcLabel = (AbcLabel)this.abcVisual;
            this.nativeTextBlock.Text = abcLabel.Text;
        }

        private void AbcLabel_FontSizeChanged(object sender, EventArgs e)
        {
            this.UpdateFontSize();
        }

        private void UpdateFontSize()
        {
            AbcLabel abcLabel = (AbcLabel)this.abcVisual;
            double fontSize = abcLabel.FontSize;

            if (fontSize > 0)
            {
                this.nativeTextBlock.FontSize = fontSize;
            }
            else
            {
                this.nativeTextBlock.ClearValue(TextBlock.FontSizeProperty);
            }
        }
    }
}