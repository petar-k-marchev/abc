using Abc.Primitives;

namespace Abc.Visuals
{
    internal class AbcLayoutContext : AbcMLContext
    {
        internal AbcRect layoutSlot;

        internal AbcLayoutContext(AbcRect slot)
        {
            this.layoutSlot = slot;
        }

        internal AbcLayoutContext(AbcLayoutContext owner)
            : base(owner)
        {
        }
    }
}
