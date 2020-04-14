using Abc.Primitives;
using Abc.Visuals;
using System;

namespace Abc
{
    internal abstract class AbcVisualTree
    {
        internal abstract AbcSize Measure(AbcVisual visual, AbcMeasureContext context);

        internal abstract void AttachToNativeParent(AbcVisual abcVisual);

        internal abstract void DetachFromNativeParent(AbcVisual abcVisual, AbcVisual oldParent);

        internal abstract void DetachFromVisualTree(AbcVisual abcVisual);

        internal virtual AbcVisual CreateVisual(Type type)
        {
            return (AbcVisual)Activator.CreateInstance(type);
        }
    }
}
