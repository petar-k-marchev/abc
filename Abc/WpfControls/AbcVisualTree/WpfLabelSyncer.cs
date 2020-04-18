using Abc.Visuals;
using System;
using System.Windows.Controls;

namespace WpfControls
{
    internal class WpfLabelSyncer : WpfVisualSyncer
    {
        private readonly AbcLabel abcLabel;
        private readonly TextBlock nativeTextBlock;

        public WpfLabelSyncer(AbcLabel abcVisual)
            : base(abcVisual, new TextBlock())
        {
            this.abcLabel = abcVisual;
            this.nativeTextBlock = (TextBlock)this.nativeVisual;
        }

        internal override void StartSync()
        {
            base.StartSync();

            this.nativeTextBlock.Text = this.abcLabel.Text;
            this.UpdateFontSize();
            this.UpdateFontSize2();

            this.abcLabel.TextChanged += this.AbcLabel_TextChanged;
            this.abcLabel.FontSize.Changed += this.AbcLabel_FontSizeChanged;
            this.abcLabel.FontSize2.Changed += this.FontSize2_Changed;
        }

        internal override void StopSync()
        {
            base.StopSync();
         
            this.abcLabel.TextChanged -= this.AbcLabel_TextChanged;
            this.abcLabel.FontSize.Changed -= this.AbcLabel_FontSizeChanged;
            this.abcLabel.FontSize2.Changed -= this.FontSize2_Changed;
        }

        private void AbcLabel_TextChanged(object sender, System.EventArgs e)
        {
            this.nativeTextBlock.Text = this.abcLabel.Text;
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
            if (this.abcLabel.FontSize.Value > 0 && !this.abcLabel.FontSize.IsDefault)
            {
                this.nativeTextBlock.FontSize = this.abcLabel.FontSize.Value;
            }
            else
            {
                this.nativeTextBlock.ClearValue(TextBlock.FontSizeProperty);
            }
        }

        private void UpdateFontSize2()
        {
            if (this.abcLabel.FontSize2.Value > 0)
            {
                this.nativeTextBlock.FontSize = this.abcLabel.FontSize2.Value;
            }
            else
            {
                this.nativeTextBlock.ClearValue(TextBlock.FontSizeProperty);
            }
        }
    }
}