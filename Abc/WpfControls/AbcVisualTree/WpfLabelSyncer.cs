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
    }
}