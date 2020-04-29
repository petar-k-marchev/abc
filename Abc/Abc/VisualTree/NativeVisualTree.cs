using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Collections.Generic;

namespace Abc
{
    internal abstract class NativeVisualTree
    {
        internal readonly Dictionary<Type, Func<IAbcVisual>> visualCreator;

        private IAbcVisual abcRoot;

        internal NativeVisualTree()
        {
            this.visualCreator = new Dictionary<Type, Func<IAbcVisual>>();
            this.visualCreator[typeof(IAbcLabel)] = CreateLabel;
            this.visualCreator[typeof(IAbcRectangle)] = CreateRectangle;
            this.visualCreator[typeof(IAbcCanvas)] = CreateCanvas;
            this.visualCreator[typeof(IAbcStack)] = CreateStack;
        }

        internal abstract bool IsAsd
        {
            get;
        }

        internal IAbcVisual AbcRoot
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

        internal abstract AbcSize Measure(IAbcVisual visual, AbcMeasureContext context);

        internal abstract void AttachToNativeParent(IAbcVisual abcVisual);

        internal abstract void DetachFromNativeParent(IAbcVisual abcVisual, IAbcVisual oldParent);

        internal abstract void DetachFromVisualTree(IAbcVisual abcVisual);

        internal virtual IAbcVisual CreateVisual(Type type)
        {
            Func<IAbcVisual> creator = this.visualCreator[type];
            IAbcVisual visual = creator();
            return visual;
        }

        internal virtual void OnAbcRootChanging(IAbcVisual newAbcRoot)
        {
            this.abcRoot = newAbcRoot;

            if (this.abcRoot != null)
            {
                this.abcRoot.VisualTree = this;
            }
        }

        private IAbcVisual CreateLabel()
        {
            return new AbcLabel();
        }

        private IAbcVisual CreateRectangle()
        {
            return new AbcRectangle();
        }

        private IAbcVisual CreateCanvas()
        {
            return new AbcCanvas();
        }

        private IAbcVisual CreateStack()
        {
            return new AbcStack();
        }
    }
}
