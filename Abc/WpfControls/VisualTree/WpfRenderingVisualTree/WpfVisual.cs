﻿using Abc;
using Abc.Controls;
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
        private bool isVisible = true;

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

        bool IAbcVisual.IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (this.isVisible != value)
                {
                    this.isVisible = value;
                    this.InvalidateMeasure();
                }
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
            if (!this.isVisible)
            {
                return;
            }

            this.isMeasurePhase = true;
            this.desiredMeasure = this.MeasureOverride(context);
            this.isMeasurePhase = false;
            this.isMeasureValid = true;
        }

        void IAbcVisual.Arrange(AbcArrangeContext context)
        {
            if (!this.isVisible)
            {
                return;
            }

            if (!this.isMeasureValid)
            {
                IAbcVisual abcVisual = this;
                abcVisual.Measure(new AbcMeasureContext(context.arrangeSlot.size, context));
            }

            this.isArrangePhase = true;
            this.arrangeSlot = context.arrangeSlot;
            this.ArrangeOverride(context);
            context.arrangeSlot = this.arrangeSlot;
            this.isArrangePhase = false;
            this.isArrangeValid = true;
        }

        void IAbcVisual.Paint(AbcArrangeContext context)
        {
            if (!this.isVisible)
            {
                return;
            }

            if (!this.isArrangeValid)
            {
                IAbcVisual abcVisual = this;
                abcVisual.Arrange(context);
            }

            this.isPaintPhase = true;
            this.PaintOverride(context);
            this.isPaintPhase = false;
        }

        void IAbcVisual.InvalidationRequestFromChild(InvalidationRequestFlag flag, IAbcVisual child)
        {
            switch (flag)
            {
                case InvalidationRequestFlag.None:
                    break;
                case InvalidationRequestFlag.Measure:
                    this.InvalidateMeasure();
                    break;
                case InvalidationRequestFlag.Arrange:
                    this.InvalidateArrange();
                    break;
                case InvalidationRequestFlag.Paint:
                    this.InvalidatePaint();
                    break;
                default:
                    break;
            }
        }

        internal virtual AbcSize MeasureOverride(AbcMeasureContext context)
        {
            return AbcSize.Zero;
        }

        internal virtual void PaintOverride(AbcArrangeContext context)
        {
            this.isPaintValid = true;
        }

        internal virtual void ArrangeOverride(AbcArrangeContext context)
        {
        }

        internal virtual void InvalidateMeasureOverride()
        {
            this.PropagateUpInvalidationRequest(InvalidationRequestFlag.Measure);
        }

        internal virtual void InvalidateArrangeOverride()
        {
            this.PropagateUpInvalidationRequest(InvalidationRequestFlag.Arrange);
        }

        internal virtual void InvalidatePaintOverride()
        {
            this.PropagateUpInvalidationRequest(InvalidationRequestFlag.Paint);
        }

        internal void InvalidateMeasure()
        {
            if (this.isMeasurePhase || this.isArrangePhase)
            {
                return;
            }

            if (this.isMeasureValid)
            {
                this.isMeasureValid = false;
                this.InvalidateMeasureOverride();
                this.InvalidateArrange();
            }
        }

        internal void InvalidateArrange()
        {
            if (this.isMeasurePhase || this.isArrangePhase)
            {
                return;
            }

            if (this.isArrangeValid)
            {
                this.isArrangeValid = false;
                this.InvalidateArrangeOverride();
                this.InvalidatePaint();
            }
        }

        internal void InvalidatePaint()
        {
            if (this.isPaintValid)
            {
                this.isPaintValid = false;
                this.InvalidatePaintOverride();
            }
        }

        private void PropagateUpInvalidationRequest(InvalidationRequestFlag flag)
        {
            if (this.visualParent != null)
            {
                this.visualParent.InvalidationRequestFromChild(flag, this);
            }
            else
            {
                IAbcVisual abcVisual = this;
                AbcContextualPropertyValue controlPropertyValue = abcVisual.GetContextualPropertyValue(AbcControlContextualProperties.ControlPropertyKey);
                IAbcControl control = (IAbcControl)(controlPropertyValue != null ? ((AbcContextualPropertyValue.AbcObject)controlPropertyValue).value : null);
                control?.RaiseInvalidationRequest(flag);
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
