using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls
{
    internal abstract class WpfVisualSyncer
    {
        internal readonly AbcVisual abcVisual;
        internal readonly UIElement nativeVisual;

        private bool isSyncing;

        protected WpfVisualSyncer(AbcVisual abcVisual, UIElement nativeVisual)
        {
            this.abcVisual = abcVisual;
            this.nativeVisual = nativeVisual;
        }

        internal bool IsSyncing
        {
            get
            {
                return this.isSyncing;
            }
            private set
            {
                if (this.isSyncing && value)
                {
                    throw new Exception(string.Format("You shouldn't need to call {0}() twice.", nameof(StartSync)));
                }

                this.isSyncing = value;
            }
        }

        internal virtual void StartSync()
        {
            this.IsSyncing = true;

            this.UpdateLayoutSlot(this.abcVisual.GetContextualPropertyValue(AbcCanvas.LayoutSlotPropertyKey));
            this.abcVisual.ContextualPropertyValueChanged += this.AbcVisual_ContextualPropertyValueChanged;
        }

        internal virtual void StopSync()
        {
            this.IsSyncing = false;

            this.abcVisual.ContextualPropertyValueChanged -= this.AbcVisual_ContextualPropertyValueChanged;
        }

        private void AbcVisual_ContextualPropertyValueChanged(object sender, AbcVisual.ContextualPropertyValueChangedEventArgs args)
        {
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