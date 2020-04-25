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

            this.UpdateArrangeSlot(this.abcVisual.GetContextualPropertyValue(AbcCanvasContextualProperties.ArrangeSlotPropertyKey));
        }

        internal override void OnContextualPropertyValueChanged(AbcContextualPropertyValueChangedEventArgs args)
        {
            base.OnContextualPropertyValueChanged(args);

            if (args.propertyKey == AbcCanvasContextualProperties.ArrangeSlotPropertyKey)
            {
                this.UpdateArrangeSlot(args.newPropertyValue);
            }
        }

        private void UpdateArrangeSlot(AbcContextualPropertyValue arrangeSlotPropertyValue)
        {
            AbcRect arrangeSlot = arrangeSlotPropertyValue != null ? ((AbcContextualPropertyValue.AbcRect)arrangeSlotPropertyValue).value : new AbcRect();
            Canvas.SetLeft(this.nativeVisual, arrangeSlot.x);
            Canvas.SetTop(this.nativeVisual, arrangeSlot.y);
            FrameworkElement frameworkElement = (FrameworkElement)this.nativeVisual;
            frameworkElement.Width = arrangeSlot.size.width;
            frameworkElement.Height = arrangeSlot.size.height;
        }
    }
}