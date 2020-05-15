using Abc.Visuals;
using System;

namespace Abc.Controls
{
    internal interface IAbcControl
    {
        event EventHandler<InvalidationRequestEventArgs> InvalidationRequest;

        AbcVisualTree VisualTree { get; set; }
        IAbcVisual Root { get; }

        void Measure(AbcMeasureContext context);
        void Arrange(AbcArrangeContext context);
        void Paint(AbcArrangeContext context);
        void RaiseInvalidationRequest(InvalidationRequestFlag flag);
    }

    internal static class AbcControlContextualProperties
    {
        internal static readonly AbcContextualPropertyKey ControlPropertyKey = new AbcContextualPropertyKey();
    }
}
