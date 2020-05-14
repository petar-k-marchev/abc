namespace Abc.Visuals
{
    internal class AbcMeasureContext : AbcContextBase
    {
        internal AbcSize availableSize;

        internal AbcMeasureContext(AbcSize availableSize)
        {
            this.availableSize = availableSize;
        }

        internal AbcMeasureContext(AbcMeasureContext owner)
            : base(owner)
        {
        }
    }
}
