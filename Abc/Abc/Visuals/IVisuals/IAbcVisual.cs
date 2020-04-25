using Abc.Primitives;
using System;

namespace Abc.Visuals
{
    internal interface IAbcVisual
    {
        event EventHandler<AbcContextualPropertyValueChangedEventArgs> ContextualPropertyValueChanged;

        IAbcVisual VisualParent { get; set; }

        NativeVisualTree VisualTree { get; set; }

        AbcSize DesiredMeasure { get; }

        AbcRect ArrangeSlot { get; }

        void Measure(AbcMeasureContext context);

        void Arrange(AbcArrangeContext context);

        AbcContextualPropertyValue GetContextualPropertyValue(AbcContextualPropertyKey propertyKey);

        void SetContextualPropertyValue(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue propertyValue);
    }
}
