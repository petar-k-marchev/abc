using Abc.Visuals;
using System.Windows;

namespace WpfControls
{
    internal abstract class WpfVisualSyncer
    {
        internal readonly AbcVisual abcVisual;
        internal readonly UIElement nativeVisual;

        protected WpfVisualSyncer(AbcVisual abcVisual, UIElement nativeVisual)
        {
            this.abcVisual = abcVisual;
            this.nativeVisual = nativeVisual;
        }
    }
}