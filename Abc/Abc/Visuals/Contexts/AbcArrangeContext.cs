namespace Abc.Visuals
{
    internal class AbcArrangeContext : AbcContextBase
    {
        internal AbcRect arrangeSlot;

        internal AbcArrangeContext(AbcRect slot)
        {
            this.arrangeSlot = slot;
        }

        internal AbcArrangeContext(AbcArrangeContext owner)
            : base(owner)
        {
        }
    }
}
