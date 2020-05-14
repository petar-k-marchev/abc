using System;

namespace Abc
{
    public class ItemPropertyChangedEventArgs<T> : EventArgs
    {
        public readonly T Item;
        public readonly string PropertyName;

        internal ItemPropertyChangedEventArgs(T item, string propertyName)
        {
            this.Item = item;
            this.PropertyName = propertyName;
        }
    }
}
