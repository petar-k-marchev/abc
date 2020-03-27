using System.Collections.Generic;

namespace Abc.Visuals
{
    internal class AbcBag
    {
        private static readonly Dictionary<string, AbcBagKey> objectIdentifierToBagKey;

        private Dictionary<AbcBagKey, object> bag;

        static AbcBag()
        {
            objectIdentifierToBagKey = new Dictionary<string, AbcBagKey>();
        }

        internal static AbcBagKey GetBagKey(string objectIdentifier)
        {
            AbcBagKey bagKey;
            if (!objectIdentifierToBagKey.TryGetValue(objectIdentifier, out bagKey))
            {
                bagKey = AbcBagKey.GetNextBagKey();
                objectIdentifierToBagKey[objectIdentifier] = bagKey;
            }

            return bagKey;
        }

        internal bool TryGetBagObject(string objectIdentifier, out object bagObject)
        {
            return TryGetBagObject(GetBagKey(objectIdentifier), out bagObject);
        }

        internal bool TryGetBagObject(AbcBagKey key, out object bagObject)
        {
            if (this.bag != null)
            {
                return this.bag.TryGetValue(key, out bagObject);
            }
            else
            {
                bagObject = null;
                return false;
            }
        }

        internal void SetBagObject(AbcBagKey key, object value)
        {
            if (this.bag == null)
            {
                lock (this)
                {
                    if (this.bag == null)
                    {
                        this.bag = new Dictionary<AbcBagKey, object>();
                    }
                }
            }

            this.bag[key] = value;
        }
    }
}
