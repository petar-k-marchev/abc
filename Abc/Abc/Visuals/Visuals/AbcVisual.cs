using Abc.Primitives;
using System;
using System.Collections.Generic;

namespace Abc.Visuals
{
    internal abstract class AbcVisual
    {
        private AbcVisual visualParent;
        private NativeVisualTree visualTree;
        private Dictionary<int, AbcContextualPropertyValue> contextualProperties;
        private bool isMeasurePhase;
        private AbcSize desiredMeasure;
        private bool isMeasureValid;
        private bool isLayoutPhase;
        private AbcRect layoutSlot;
        private bool isLayoutValid;

        internal event EventHandler<ContextualPropertyValueChangedEventArgs> ContextualPropertyValueChanged;

        //// Tips for woking with visual tree and visual parent.
        //// No automatic propagation for the visual tree.
        //// Set the visual parent always to the actual corresponding parent. 
        //// Set the visual tree only if 
        ////    A) The parent has a visual tree.
        //// or
        ////    B) You want to remove the visual (and discard all native bits) from the visual tree.

        internal AbcVisual VisualParent
        {
            get
            {
                return this.visualParent;
            }
            set
            {
                if (this.visualParent == value)
                {
                    throw new Exception(string.Format("Setting the {0} to the same vlaue seems unneeded.", nameof(VisualParent)));
                }

                if (this.visualParent != null && value != null)
                {
                    throw new InvalidOperationException(string.Format("Visual is already attached to a {0}.", nameof(VisualParent)));
                }

                AbcVisual oldVisualParent = this.visualParent;
                this.visualParent = value;
                this.OnVisualParentChanged(oldVisualParent);
            }
        }

        internal NativeVisualTree VisualTree
        {
            get
            {
                return this.visualTree;
            }
            set
            {
                if (this.visualTree == value)
                {
                    throw new Exception(string.Format("You shouldn't need to set the {0} twice.", nameof(VisualTree)));
                }

                NativeVisualTree oldVisualTree = this.visualTree;
                this.visualTree = value;
                this.OnVisualTreeChanged(oldVisualTree);
            }
        }

        internal AbcSize DesiredMeasure
        {
            get
            {
                return this.desiredMeasure;
            }
        }

        internal AbcRect LayoutSlot
        {
            get
            {
                return this.layoutSlot;
            }
        }

        protected virtual AbcSize MeasureOverride(AbcMeasureContext context)
        {
            AbcSize size = this.VisualTree.Measure(this, context);
            return size;
        }

        protected virtual void LayoutOverride(AbcLayoutContext context)
        {
        }

        protected virtual void OnVisualParentChanged(AbcVisual oldVisualParent)
        {
            if (this.VisualTree != null)
            {
                if (oldVisualParent != null)
                {
                    this.VisualTree.DetachFromNativeParent(this, oldVisualParent);
                }

                this.VisualTree.AttachToNativeParent(this);
            }
        }

        protected virtual void OnVisualTreeChanged(NativeVisualTree oldVisualTree)
        {
            if (oldVisualTree != null)
            {
                oldVisualTree.DetachFromVisualTree(this);
            }

            if (this.VisualParent != null)
            {
                this.VisualTree.AttachToNativeParent(this);
            }
        }

        internal void Measure(AbcMeasureContext context)
        {
            if (this.isMeasureValid)
            {
                return;
            }

            this.isMeasurePhase = true;
            this.desiredMeasure = this.MeasureOverride(context);
            this.isMeasurePhase = false;
            this.isMeasureValid = true;
        }

        internal void Layout(AbcLayoutContext context)
        {
            if (this.isLayoutValid)
            {
                this.isLayoutValid = this.layoutSlot == context.layoutSlot;
            }

            if (this.isLayoutValid)
            {
                return;
            }

            this.isLayoutPhase = true;
            this.layoutSlot = context.layoutSlot;
            this.LayoutOverride(context);
            this.isLayoutPhase = false;
            this.isLayoutValid = true;
        }

        internal AbcContextualPropertyValue GetContextualPropertyValue(AbcContextualPropertyKey propertyKey)
        {
            AbcContextualPropertyValue propertyValue = null;

            this.contextualProperties?.TryGetValue(propertyKey.key, out propertyValue);
            // if not present - perhaps some get-on-demand default value (propertyKey.GetDefaultPropertyValue())

            return propertyValue;
        }

        internal void SetContextualPropertyValue(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue propertyValue)
        {
            if (this.contextualProperties == null)
            {
                this.contextualProperties = new Dictionary<int, AbcContextualPropertyValue>();
            }

            EventHandler<ContextualPropertyValueChangedEventArgs> propertyChanged = this.ContextualPropertyValueChanged;
            AbcContextualPropertyValue oldPropertyValue = null;
            if (propertyChanged != null)
            {
                this.contextualProperties.TryGetValue(propertyKey.key, out oldPropertyValue);
            }

            this.contextualProperties[propertyKey.key] = propertyValue;

            if (propertyChanged != null)
            {
                propertyChanged(this, new ContextualPropertyValueChangedEventArgs(propertyKey, oldPropertyValue, propertyValue));
            }
        }

        internal void AddFlag(AbcVisualFlag flag)
        {
            if (this.isMeasureValid)
            {
                bool affectsMeasure = flag == AbcVisualFlag.AffectsMeasureOnly || flag == AbcVisualFlag.AffectsMeasureAndLayout;
            }
        }

        internal struct ContextualPropertyValueChangedEventArgs
        {
            internal readonly AbcContextualPropertyKey propertyKey;
            internal readonly AbcContextualPropertyValue oldPropertyValue;
            internal readonly AbcContextualPropertyValue newPropertyValue;

            public ContextualPropertyValueChangedEventArgs(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue oldPropertyValue, AbcContextualPropertyValue newPropertyValue)
            {
                this.propertyKey = propertyKey;
                this.oldPropertyValue = oldPropertyValue;
                this.newPropertyValue = newPropertyValue;
            }
        }
    }
}
