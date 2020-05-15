using Abc.Controls;
using System;

namespace Abc.Visuals
{
    internal interface IAbcVisual
    {
        event EventHandler<AbcContextualPropertyValueChangedEventArgs> ContextualPropertyValueChanged;

        AbcVisualTree VisualTree { get; set; }
        IAbcVisual VisualParent { get; set; }
        AbcSize DesiredMeasure { get; }
        AbcRect ArrangeSlot { get; }

        void Measure(AbcMeasureContext context);
        void Arrange(AbcArrangeContext context);
        void Paint(AbcContextBase context);
        AbcContextualPropertyValue GetContextualPropertyValue(AbcContextualPropertyKey propertyKey);
        void SetContextualPropertyValue(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue propertyValue);
        void OnChildMeasureInvalidated(IAbcVisual child);
    }
}
