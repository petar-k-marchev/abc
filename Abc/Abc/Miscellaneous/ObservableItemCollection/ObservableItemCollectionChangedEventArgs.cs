using System;

namespace Abc.Miscellaneous
{
    public class ObservableItemCollectionChangedEventArgs<T> : EventArgs
    {
        public readonly T Item;
        public readonly int Index;

        internal ObservableItemCollectionChangedEventArgs(T item, int index)
        {
            this.Item = item;
            this.Index = index;
        }
    }
}
