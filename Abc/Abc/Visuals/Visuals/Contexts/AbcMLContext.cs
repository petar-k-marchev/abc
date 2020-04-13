namespace Abc.Visuals
{
    internal abstract class AbcMLContext
    {
        private readonly AbcMLContext owner;

        private AbcBag bag;

        internal AbcMLContext(AbcMLContext owner = null)
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
