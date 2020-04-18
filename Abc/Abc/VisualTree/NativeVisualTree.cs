using Abc.Primitives;
using Abc.Visuals;
using System;

namespace Abc
{
    internal abstract class NativeVisualTree
    {
        private AbcVisual abcRoot;

        internal AbcVisual AbcRoot
        {
            get
            {
                return this.abcRoot;
            }
            set
            {
                if (this.abcRoot == value)
                {
                    throw new Exception(string.Format("You shouldn't need to set the {0} twice.", nameof(AbcRoot)));
                }

                this.OnAbcRootChanging(value);
            }
        }

        internal abstract AbcSize Measure(AbcVisual visual, AbcMeasureContext context);

        internal abstract void AttachToNativeParent(AbcVisual abcVisual);

        internal abstract void DetachFromNativeParent(AbcVisual abcVisual, AbcVisual oldParent);

        internal abstract void DetachFromVisualTree(AbcVisual abcVisual);

        internal virtual AbcVisual CreateVisual(Type type)
        {
            return (AbcVisual)Activator.CreateInstance(type);
        }

        internal virtual void OnAbcRootChanging(AbcVisual newAbcRoot)
        {
            this.abcRoot = newAbcRoot;

            if (this.abcRoot != null)
            {
                this.abcRoot.VisualTree = this;
            }
        }
    }
}
