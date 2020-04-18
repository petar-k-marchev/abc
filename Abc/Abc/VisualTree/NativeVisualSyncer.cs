using Abc.Primitives;
using Abc.Visuals;
using System;

namespace Abc
{
    internal abstract class NativeVisualSyncer
    {
        internal readonly AbcVisual abcVisual;

        private bool isSyncing;
        private bool hasContextualListeners;

        protected NativeVisualSyncer(AbcVisual abcVisual, bool hasContextualListeners)
        {
            this.abcVisual = abcVisual;
            this.hasContextualListeners = hasContextualListeners;
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

        internal abstract AbcSize Measure(AbcMeasureContext context);

        internal virtual void StartSync()
        {
            this.IsSyncing = true;

            if (this.hasContextualListeners)
            {
                this.abcVisual.ContextualPropertyValueChanged += this.AbcVisual_ContextualPropertyValueChanged;
            }
        }

        internal virtual void StopSync()
        {
            this.IsSyncing = false;

            if (this.hasContextualListeners)
            {
                this.abcVisual.ContextualPropertyValueChanged -= this.AbcVisual_ContextualPropertyValueChanged;
            }
        }

        internal virtual void OnContextualPropertyValueChanged(AbcVisual.ContextualPropertyValueChangedEventArgs args)
        {
        }

        private void AbcVisual_ContextualPropertyValueChanged(object sender, AbcVisual.ContextualPropertyValueChangedEventArgs args)
        {
            this.OnContextualPropertyValueChanged(args);
        }
    }
}
