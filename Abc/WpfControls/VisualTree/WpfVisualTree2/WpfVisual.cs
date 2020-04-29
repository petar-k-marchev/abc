using Abc;
using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfVisual : IAbcVisual
    {
        internal readonly UIElement uiElement;

        private event EventHandler<AbcContextualPropertyValueChangedEventArgs> contextualPropertyValueChanged;

        private IAbcVisual visualParent;
        private NativeVisualTree visualTree;
        private Dictionary<int, AbcContextualPropertyValue> contextualProperties;
        private bool isArrangePhase;
        private AbcRect arrangeSlot;
        private bool isArrangeValid;

        internal WpfVisual(UIElement uiElement)
        {
            this.uiElement = uiElement;
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

        AbcControlInfo IAbcVisual.ControlInfo
        {
            get;
            set;
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

        NativeVisualTree IAbcVisual.VisualTree
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

                NativeVisualTree oldVisualTree = this.visualTree;
                this.visualTree = value;
                this.OnVisualTreeChanged(oldVisualTree);
            }
        }

        AbcSize IAbcVisual.DesiredMeasure
        {
            get
            {
                return new AbcSize(this.uiElement.DesiredSize.Width, this.uiElement.DesiredSize.Height);
            }
        }

        AbcRect IAbcVisual.ArrangeSlot
        {
            get
            {
                return this.arrangeSlot;
            }
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

        AbcContextualPropertyValue IAbcVisual.GetContextualPropertyValue(AbcContextualPropertyKey propertyKey)
        {
            AbcContextualPropertyValue propertyValue = null;

            this.contextualProperties?.TryGetValue(propertyKey.key, out propertyValue);
            // if not present - perhaps some get-on-demand default value (propertyKey.GetDefaultPropertyValue())

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
            IAbcVisual abcVisual = this;
            if (abcVisual.ControlInfo != null)
            {
                if (abcVisual.ControlInfo.controlType == AbcControlType.Slave)
                {
                    // i am sleepy now
                }
            }

            this.uiElement.Measure(new Size(context.availableSize.width, context.availableSize.height));
        }

        internal virtual void ArrangeOverride(AbcArrangeContext context)
        {
            Rect finalRect = new Rect(this.arrangeSlot.x, this.arrangeSlot.y, this.arrangeSlot.size.width, this.arrangeSlot.size.height);
            this.uiElement.Arrange(finalRect);
        }

        protected virtual void OnVisualParentChanged(IAbcVisual oldVisualParent)
        {
            this.RemoveFromParent(oldVisualParent);
            this.AddToParent();
        }

        protected virtual void OnVisualTreeChanged(NativeVisualTree oldVisualTree)
        {
        }

        private void AddToParent()
        {
            IAbcVisual abcVisual = this;
            IAbcVisual abcVisualParent = abcVisual.VisualParent;

            if (abcVisualParent == null)
            {
                return;
            }

            WpfVisual wpfVisualParent = null;

            if (abcVisual.ControlInfo != null)
            {
                if (abcVisual.ControlInfo.controlType == AbcControlType.Slave)
                {
                    return;
                }
                else
                {
                    wpfVisualParent = (WpfVisual)abcVisual.ControlInfo.counterpart;
                }
                return;
                wpfVisualParent = (WpfVisual)abcVisual.ControlInfo.counterpart;
            }
            else
            {
                wpfVisualParent = (WpfVisual)abcVisualParent;
            }

            Panel parentPanel = (Panel)wpfVisualParent.uiElement;
            parentPanel.Children.Add(this.uiElement);
        }

        private void RemoveFromParent(IAbcVisual oldVisualParent)
        {
            if (oldVisualParent == null)
            {
                return;
            }

            WpfVisual wpfVisualParent = (WpfVisual)oldVisualParent;
            Panel parentPanel = (Panel)wpfVisualParent.uiElement;
            parentPanel.Children.Remove(this.uiElement);
        }
    }
}