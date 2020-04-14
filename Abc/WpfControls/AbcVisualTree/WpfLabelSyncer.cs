using Abc.Visuals;
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
            this.abcLabel.TextChanged += this.AbcLabel_TextChanged;
        }

        internal override void StopSync()
        {
            base.StopSync();
         
            this.abcLabel.TextChanged -= this.AbcLabel_TextChanged;
        }

        private void AbcLabel_TextChanged(object sender, System.EventArgs e)
        {
            this.nativeTextBlock.Text = this.abcLabel.Text;
        }
    }
}