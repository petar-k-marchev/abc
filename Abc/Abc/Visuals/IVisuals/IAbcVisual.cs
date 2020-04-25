using Abc.Primitives;
using System;

namespace Abc.Visuals
{
    internal interface IAbcVisual
    {
        event EventHandler<AbcContextualPropertyValueChangedEventArgs> ContextualPropertyValueChanged;

        AbcVisual VisualParent { get; set; }

        NativeVisualTree VisualTree { get; set; }

        AbcSize DesiredMeasure { get; }

        AbcRect LayoutSlot { get; }

        void Measure(AbcMeasureContext context);

        void Layout(AbcLayoutContext context);

        AbcContextualPropertyValue GetContextualPropertyValue(AbcContextualPropertyKey propertyKey);

        void SetContextualPropertyValue(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue propertyValue);
    }
}
