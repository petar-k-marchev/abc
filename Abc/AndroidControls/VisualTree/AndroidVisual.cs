using System;
using System.Collections.Generic;
using Abc;
using Abc.Controls;
using Abc.Visuals;
using Android.Views;

namespace AndroidControls.VisualTree
{
    internal class AndroidVisual : IAbcVisual
    {
        internal readonly View view;

        private event EventHandler<AbcContextualPropertyValueChangedEventArgs> contextualPropertyValueChanged;

        private IAbcVisual visualParent;
        private AbcVisualTree visualTree;
        private Dictionary<int, AbcContextualPropertyValue> contextualProperties;
        private bool isMeasurePhase;
        private bool isMeasureValid;
        private bool isArrangePhase;
        private AbcRect arrangeSlot;
        private bool isArrangeValid;
        private bool isPaintPhase;
        private bool isPaintValid;

        public AndroidVisual(View view)
        {
            this.view = view;
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
                    throw new Exception(string.Format("You shouldn't need to set the {0} twice.", nameof(IAbcVisual.VisualTree)));
                }

                AbcVisualTree oldVisualTree = this.visualTree;
                this.visualTree = value;
                this.OnVisualTreeChanged(oldVisualTree);
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

        AbcSize IAbcVisual.DesiredMeasure
        {
            get
            {
                return new AbcSize(this.view.MeasuredWidth, this.view.MeasuredHeight);
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
                return this.view.Visibility == ViewStates.Visible;
            }
            set
            {
                var isVisible = this.view.Visibility == ViewStates.Visible;
                if (isVisible != value)
                {
                    this.view.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
                    this.InvalidateMeasure();
                }
            }
        }

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

        void IAbcVisual.Arrange(AbcArrangeContext context)
        {
            IAbcVisual abcVisual = this;
            if (!abcVisual.IsVisible)
            {
                return;
            }

            if (!this.isMeasureValid)
            {
                abcVisual.Measure(new AbcMeasureContext(context.arrangeSlot.size, context));
            }

            this.isArrangePhase = true;
            this.arrangeSlot = context.arrangeSlot;
            this.ArrangeOverride(context);
            context.arrangeSlot = this.arrangeSlot;
            this.isArrangePhase = false;
            this.isArrangeValid = true;
        }

        void IAbcVisual.Measure(AbcMeasureContext context)
        {
            IAbcVisual abcVisual = this;
            if (!abcVisual.IsVisible)
            {
                return;
            }

            this.isMeasurePhase = true;
            this.MeasureOverride(context);
            this.isMeasurePhase = false;
            this.isMeasureValid = true;
        }

        void IAbcVisual.Paint(AbcArrangeContext context)
        {
            IAbcVisual abcVisual = this;
            if (!abcVisual.IsVisible)
            {
                return;
            }

            if (!this.isArrangeValid)
            {
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
            propertyChanged?.Invoke(this, new AbcContextualPropertyValueChangedEventArgs(propertyKey, oldPropertyValue, propertyValue));
        }

        internal virtual void MeasureOverride(AbcMeasureContext context)
        {
            this.view.Measure((int)context.availableSize.width, (int)context.availableSize.height);
        }

        internal virtual void ArrangeOverride(AbcArrangeContext context)
        {
            var finalRect = this.arrangeSlot;
            this.view.Layout((int)finalRect.x, (int)finalRect.y, (int)finalRect.Right(), (int)finalRect.Bottom());
        }

        internal virtual void PaintOverride(AbcArrangeContext context)
        {
            this.isPaintValid = true;
        }

        internal void InvalidateMeasure()
        {
            if (this.isMeasurePhase || this.isArrangePhase)
            {
                return;
            }

            this.view.RequestLayout();

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

            this.view.Invalidate();

            if (this.isArrangeValid)
            {
                this.isArrangeValid = false;
                this.InvalidateArrangeOverride();
                this.InvalidatePaint();
            }
        }

        internal void InvalidatePaint()
        {
            this.view.Invalidate();

            if (this.isPaintValid)
            {
                this.InvalidatePaintOverride();
                this.isPaintValid = false;
            }
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

        protected virtual void OnVisualTreeChanged(AbcVisualTree oldVisualTree)
        {
        }

        protected virtual void OnVisualParentChanged(IAbcVisual oldVisualParent)
        {
            this.RemoveFromParent(oldVisualParent);
            this.AddToParent();
        }

        private void AddToParent()
        {
            IAbcVisual abcVisual = this;
            IAbcVisual abcVisualParent = abcVisual.VisualParent;

            if (abcVisualParent == null)
            {
                return;
            }

            var androidVisualParent = (AndroidVisual)abcVisualParent;
            var parentPanel = (ViewGroup)androidVisualParent.view;
            parentPanel.AddView(this.view);
        }

        private void RemoveFromParent(IAbcVisual oldVisualParent)
        {
            if (oldVisualParent == null)
            {
                return;
            }

            var androidVisualParent = (AndroidVisual)oldVisualParent;
            var parentPanel = (ViewGroup)androidVisualParent.view;
            parentPanel.RemoveView(this.view);
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
    }
}
