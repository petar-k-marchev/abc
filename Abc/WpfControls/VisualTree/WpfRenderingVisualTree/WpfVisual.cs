using Abc;
using Abc.Visuals;
using System;
using System.Collections.Generic;

namespace WpfControls.WpfRenderingVisualTreeInternals
{
    internal abstract class WpfVisual : IAbcVisual
    {
        private event EventHandler<AbcContextualPropertyValueChangedEventArgs> contextualPropertyValueChanged;

        private IAbcVisual visualParent;
        private AbcVisualTree visualTree;
        private Dictionary<int, AbcContextualPropertyValue> contextualProperties;
        private AbcSize desiredMeasure;
        private bool isMeasurePhase;
        private bool isMeasureValid;
        private bool isArrangePhase;
        private AbcRect arrangeSlot;
        private bool isArrangeValid;
        private bool isPaintPhase;
        private bool isPaintValid;

        event EventHandler<AbcContextualPropertyValueChangedEventArgs> IAbcVisual.ContextualPropertyValueChanged
        {
            add
            {
                this.contextualPropertyValueChanged += value;
            }
            remove
            {
                this.contextualPropertyValueChanged -= value;
            }
        }

        IAbcVisual IAbcVisual.VisualParent
        {
            get
            {
                return this.visualParent;
            }
            set
            {
                if (this.visualParent == value)
                {
                    throw new Exception(string.Format("Setting the {0} to the same vlaue seems unneeded.", nameof(IAbcVisual.VisualParent)));
                }

                if (this.visualParent != null && value != null)
                {
                    throw new InvalidOperationException(string.Format("Visual is already attached to a {0}.", nameof(IAbcVisual.VisualParent)));
                }

                IAbcVisual oldVisualParent = this.visualParent;
                this.visualParent = value;
                this.OnVisualParentChanged(oldVisualParent);
            }
        }

        AbcVisualTree IAbcVisual.VisualTree
        {
            get
            {
                return this.visualTree;
            }
            set
            {
                if (this.visualTree == value)
                {
                    //temporary disable while testing IAbcControl
                    //throw new Exception(string.Format("You shouldn't need to set the {0} twice.", nameof(IAbcVisual.VisualTree)));
                    return;
                }

                AbcVisualTree oldVisualTree = this.visualTree;
                this.visualTree = value;
                this.OnVisualTreeChanged(oldVisualTree);
            }
        }

        AbcSize IAbcVisual.DesiredMeasure
        {
            get
            {
                return this.desiredMeasure;
            }
        }

        AbcRect IAbcVisual.ArrangeSlot
        {
            get
            {
                return this.arrangeSlot;
            }
        }

        AbcContextualPropertyValue IAbcVisual.GetContextualPropertyValue(AbcContextualPropertyKey propertyKey)
        {
            AbcContextualPropertyValue propertyValue = null;
            this.contextualProperties?.TryGetValue(propertyKey.key, out propertyValue);
            return propertyValue;
        }

        void IAbcVisual.SetContextualPropertyValue(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue propertyValue)
        {
            if (this.contextualProperties == null)
            {
                this.contextualProperties = new Dictionary<int, AbcContextualPropertyValue>();
            }

            EventHandler<AbcContextualPropertyValueChangedEventArgs> propertyChanged = this.contextualPropertyValueChanged;
            AbcContextualPropertyValue oldPropertyValue = null;
            if (propertyChanged != null)
            {
                this.contextualProperties.TryGetValue(propertyKey.key, out oldPropertyValue);
            }

            this.contextualProperties[propertyKey.key] = propertyValue;

            if (propertyChanged != null)
            {
                propertyChanged(this, new AbcContextualPropertyValueChangedEventArgs(propertyKey, oldPropertyValue, propertyValue));
            }
        }

        void IAbcVisual.Measure(AbcMeasureContext context)
        {
            this.isMeasurePhase = true;
            this.desiredMeasure = this.MeasureOverride(context);
            this.isMeasurePhase = false;
            this.isMeasureValid = true;
        }

        void IAbcVisual.Arrange(AbcArrangeContext context)
        {
            this.isArrangePhase = true;
            this.arrangeSlot = context.arrangeSlot;
            this.ArrangeOverride(context);
            context.arrangeSlot = this.arrangeSlot;
            this.isArrangePhase = false;
            this.isArrangeValid = true;
        }

        void IAbcVisual.Paint(AbcContextBase context)
        {
            this.isPaintPhase = true;
            this.PaintOverride(context);
            this.isPaintPhase = false;
        }

        internal virtual AbcSize MeasureOverride(AbcMeasureContext context)
        {
            return AbcSize.Zero;
        }

        internal virtual void PaintOverride(AbcContextBase context)
        {
        }

        internal virtual void ArrangeOverride(AbcArrangeContext context)
        {
        }

        internal virtual void InvalidateMeasureOverride()
        {
            // notify parent measure is invalid
        }

        internal virtual void InvalidateArrangeOverride()
        {
            // notify parent arrange is invalid
        }

        internal virtual void InvalidatePaintOverride()
        {
            // notify parent paint is invalid
        }

        internal void InvalidateMeasure()
        {
            if (!this.isMeasureValid)
            {
                this.isMeasureValid = false;
                this.InvalidateMeasureOverride();
                this.InvalidateArrange();
                this.InvalidatePaint();
            }
        }

        internal void InvalidateArrange()
        {
            if (!this.isArrangeValid)
            {
                this.isArrangeValid = false;
                this.InvalidateArrangeOverride();
                this.InvalidatePaint();
            }
        }

        internal void InvalidatePaint()
        {
            if (!this.isPaintValid)
            {
                this.isPaintValid = false;
                this.InvalidatePaintOverride();
            }
        }

        protected virtual void OnVisualParentChanged(IAbcVisual oldVisualParent)
        {
        }

        protected virtual void OnVisualTreeChanged(AbcVisualTree oldVisualTree)
        {
        }
    }
}
