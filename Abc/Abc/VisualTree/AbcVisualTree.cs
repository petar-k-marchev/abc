using Abc.Primitives;
using Abc.Visuals;
using System;

namespace Abc
{
    internal abstract class AbcVisualTree
    {
        internal abstract AbcSize Measure(AbcVisual visual, AbcMeasureContext context);

        /// <summary>
        /// Detach from old parent and if there is a new parent, then attach to it.
        /// </summary>
        internal abstract void AttachToNativeParent(AbcVisual abcVisual, AbcVisual oldVisualParent);

        internal virtual AbcVisual CreateVisual(Type type)
        {
            return (AbcVisual)Activator.CreateInstance(type);
        }
    }
}
