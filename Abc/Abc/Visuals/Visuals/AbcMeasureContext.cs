namespace Abc.Visuals
{
    internal class AbcMeasureContext
    {
        internal readonly AbcMeasureContext owner;

        internal AbcSize availableSize;

        private AbcBag bag;

        internal AbcMeasureContext(AbcSize availableSize)
        {
            this.availableSize = availableSize;
        }

        internal AbcMeasureContext(AbcMeasureContext owner)
        {
            this.owner = owner;
        }

        internal AbcBag Bag
        {
            get
            {
                if (this.owner != null)
                {
                    return this.owner.Bag;
                }

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
