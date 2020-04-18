using Abc;
using Abc.Primitives;
using Abc.Visuals;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls.WpfVisualTreeInternals
{
    internal abstract class WpfVisualSyncer : NativeVisualSyncer
    {
        internal readonly UIElement nativeVisual;

        protected WpfVisualSyncer(AbcVisual abcVisual, UIElement nativeVisual)
            : base(abcVisual, hasContextualListeners: true)
        {
            this.nativeVisual = nativeVisual;
        }

        internal override AbcSize Measure(AbcMeasureContext context)
        {
            FrameworkElement frameworkElement = (FrameworkElement)this.nativeVisual;
            frameworkElement.Width = double.NaN;
            frameworkElement.Height = double.NaN;

            nativeVisual.Measure(new Size(context.availableSize.width, context.availableSize.height));
            return new AbcSize(nativeVisual.DesiredSize.Width, nativeVisual.DesiredSize.Height);
        }

        internal override void StartSync()
        {
            base.StartSync();

            this.UpdateLayoutSlot(this.abcVisual.GetContextualPropertyValue(AbcCanvas.LayoutSlotPropertyKey));
        }

        internal override void OnContextualPropertyValueChanged(AbcVisual.ContextualPropertyValueChangedEventArgs args)
        {
            base.OnContextualPropertyValueChanged(args);

            if (args.propertyKey == AbcCanvas.LayoutSlotPropertyKey)
            {
                this.UpdateLayoutSlot(args.newPropertyValue);
            }
        }

        private void UpdateLayoutSlot(AbcContextualPropertyValue layoutSlotPropertyValue)
        {
            AbcRect layoutSlot = layoutSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)layoutSlotPropertyValue).value : new AbcRect();
            Canvas.SetLeft(this.nativeVisual, layoutSlot.x);
            Canvas.SetTop(this.nativeVisual, layoutSlot.y);
            FrameworkElement frameworkElement = (FrameworkElement)this.nativeVisual;
            frameworkElement.Width = layoutSlot.size.width;
            frameworkElement.Height = layoutSlot.size.height;
        }
    }
}