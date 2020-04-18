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
            this.UpdateFontSize2();

            abcLabel.TextChanged += this.AbcLabel_TextChanged;
            abcLabel.FontSize.Changed += this.AbcLabel_FontSizeChanged;
            abcLabel.FontSize2.Changed += this.FontSize2_Changed;
        }

        internal override void StopSync()
        {
            base.StopSync();

            AbcLabel abcLabel = (AbcLabel)this.abcVisual;

            abcLabel.TextChanged -= this.AbcLabel_TextChanged;
            abcLabel.FontSize.Changed -= this.AbcLabel_FontSizeChanged;
            abcLabel.FontSize2.Changed -= this.FontSize2_Changed;
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

        private void FontSize2_Changed(object sender, AbcProperty<double>.AbcPropertyChangedEventArgs e)
        {
            this.UpdateFontSize2();
        }

        private void UpdateFontSize()
        {
            AbcLabel abcLabel = (AbcLabel)this.abcVisual;

            if (abcLabel.FontSize.Value > 0 && !abcLabel.FontSize.IsDefault)
            {
                this.nativeTextBlock.FontSize = abcLabel.FontSize.Value;
            }
            else
            {
                this.nativeTextBlock.ClearValue(TextBlock.FontSizeProperty);
            }
        }

        private void UpdateFontSize2()
        {
            AbcLabel abcLabel = (AbcLabel)this.abcVisual;

            if (abcLabel.FontSize2.Value > 0)
            {
                this.nativeTextBlock.FontSize = abcLabel.FontSize2.Value;
            }
            else
            {
                this.nativeTextBlock.ClearValue(TextBlock.FontSizeProperty);
            }
        }
    }
}