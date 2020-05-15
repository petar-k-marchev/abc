using Abc.Visuals;
using System;

namespace Abc.Controls
{
    internal interface IAbcControl
    {
        event EventHandler<InvalidationRequestEventArgs> InvalidationRequest;

        AbcVisualTree VisualTree { get; set; }
        IAbcVisual Root { get; }

        void RaiseInvalidationRequest(InvalidationRequestFlag flag);
    }

    internal static class AbcControlContextualProperties
    {
        internal static readonly AbcContextualPropertyKey ControlPropertyKey = new AbcContextualPropertyKey();
    }
}
