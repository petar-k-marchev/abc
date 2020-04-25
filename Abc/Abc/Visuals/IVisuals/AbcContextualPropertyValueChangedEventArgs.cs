namespace Abc.Visuals
{
    internal struct AbcContextualPropertyValueChangedEventArgs
    {
        internal readonly AbcContextualPropertyKey propertyKey;
        internal readonly AbcContextualPropertyValue oldPropertyValue;
        internal readonly AbcContextualPropertyValue newPropertyValue;

        public AbcContextualPropertyValueChangedEventArgs(AbcContextualPropertyKey propertyKey, AbcContextualPropertyValue oldPropertyValue, AbcContextualPropertyValue newPropertyValue)
        {
            this.propertyKey = propertyKey;
            this.oldPropertyValue = oldPropertyValue;
            this.newPropertyValue = newPropertyValue;
        }
    }
}
