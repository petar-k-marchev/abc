using Abc.Visuals;
using System;
using System.Collections.Generic;

namespace Abc
{
    internal abstract class AbcVisualTree
    {
        protected readonly Dictionary<Type, Func<IAbcVisual>> visualCreator = new Dictionary<Type, Func<IAbcVisual>>();

        internal virtual IAbcVisual CreateVisual(Type abcVisualType)
        {
            Func<IAbcVisual> creator = visualCreator[abcVisualType];
            IAbcVisual visual = creator();
            return visual;
        }
    }
}
