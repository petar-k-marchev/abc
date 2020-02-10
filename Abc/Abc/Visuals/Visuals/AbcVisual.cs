using System.Collections.Generic;
using System.Threading;

namespace Abc.Visuals
{
    internal abstract class AbcVisual
    {
        private static int givenBagKeyValues;
        private static Dictionary<object, AbcBagKey> objectIdentifierToBagKey;
        
        private Dictionary<AbcBagKey, object> bag;
        private Dictionary<int, AbcContextualPropertyValue> contextualProperties;

        internal static AbcBagKey GetNextBagKey()
        {
            int value = Interlocked.Increment(ref givenBagKeyValues);
            return new AbcBagKey(value);
        }

        internal static AbcBagKey GetBagKey(object objectIdentifier)
        {
            if (objectIdentifierToBagKey == null)
            {
                objectIdentifierToBagKey = new Dictionary<object, AbcBagKey>();
            }

            AbcBagKey bagKey;
            if (!objectIdentifierToBagKey.TryGetValue(objectIdentifier, out bagKey))
            {
                bagKey = GetNextBagKey();
                objectIdentifierToBagKey[objectIdentifier] = bagKey;
            }

            return bagKey;
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
                this.bag = new Dictionary<AbcBagKey, object>();
            }

            this.bag[key] = value;
        }

        internal AbcContextualPropertyValue GetContextualPropertyValue(AbcContextualPropertyKey propertyKey)
        {
            AbcContextualPropertyValue propertyValue = null;

            this.contextualProperties?.TryGetValue(propertyKey.key, out propertyValue);
            // if not present - perhaps some get-on-demand default value (propertyKey.GetDefaultPropertyValue())

            return propertyValue;
        }

        internal void SetContextualPropertyValue(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue propertyValue)
        {
            if (this.contextualProperties == null)
            {
                this.contextualProperties = new Dictionary<int, AbcContextualPropertyValue>();
            }

            this.contextualProperties[propertyKey.key] = propertyValue;

            // property changed notifications ?
        }
    }
}
