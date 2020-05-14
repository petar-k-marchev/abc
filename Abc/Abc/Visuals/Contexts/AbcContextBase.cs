namespace Abc.Visuals
{
    internal class AbcContextBase
    {
        private readonly AbcContextBase owner;

        private AbcBag bag;

        internal AbcContextBase(AbcContextBase owner = null)
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
