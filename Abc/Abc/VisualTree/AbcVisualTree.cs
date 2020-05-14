using Abc.Visuals;
using System;
using System.Collections.Generic;

namespace Abc
{
    internal abstract class AbcVisualTree
    {
        protected readonly Dictionary<Type, Func<IAbcVisual>> visualCreator = new Dictionary<Type, Func<IAbcVisual>>();

        private AbcBag bag;

        internal virtual IAbcVisual CreateVisual(Type abcVisualType)
        {
            Func<IAbcVisual> creator = visualCreator[abcVisualType];
            IAbcVisual visual = creator();
            return visual;
        }

        internal AbcBag Bag
        {
            get
            {
                if (this.bag == null)
                {
                    lock (this)
                    {
                        if (this.bag == null)
                        {
                            this.bag = new AbcBag();
                        }
                    }
                }

                return this.bag;
            }
        }
    }
}
