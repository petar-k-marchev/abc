using System.Collections.Generic;

namespace Abc.Visuals
{
    internal abstract class AbcVisualsContainer : AbcVisual
    {
        internal readonly IList<AbcVisual> children = new List<AbcVisual>();
    }
}
