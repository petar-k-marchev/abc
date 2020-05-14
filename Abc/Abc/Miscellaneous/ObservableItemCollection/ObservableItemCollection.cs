using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Abc
{
    public class ObservableItemCollection<T> : ObservableCollection<T>
    {
        private List<T> backup;
        private bool hasItemPropertyChangedListeners;

        public ObservableItemCollection()
        {
            this.backup = new List<T>();
            this.CollectionChanged += this.ObservedCollectionChanged;
        }
        
        public event EventHandler<ObservableItemCollectionChangedEventArgs<T>> ItemAdded;
        public event EventHandler<ObservableItemCollectionChangedEventArgs<T>> ItemRemoved;

        public event EventHandler<ItemPropertyChangedEventArgs<T>> ItemPropertyChanged
        {
            add
            {
                this.itemPropertyChanged += value;

                if (!this.hasItemPropertyChangedListeners)
                {
                    this.hasItemPropertyChangedListeners = true;
                    this.AttachItemsPropertyChanged();
                }
            }
            remove
            {
                this.itemPropertyChanged -= value;

                if (this.itemPropertyChanged == null)
                {
                    this.DetachItemsPropertyChanged();
                    this.hasItemPropertyChangedListeners = false;
                }
            }
        }

        private event EventHandler<ItemPropertyChangedEventArgs<T>> itemPropertyChanged;

        protected override void ClearItems()
        {
            if (this.Count == 0)
            {
                //// Do not call base.ClearItems() because it will raise CollectionChanged with action Reset 
                //// even when the collection is empty.
                return;
            }

            this.backup.AddRange(this);

            base.ClearItems();
        }

        private void ObservedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IList oldItems = e.Action == NotifyCollectionChangedAction.Reset ? this.backup : e.OldItems;
            int oldStartingIndex = e.Action == NotifyCollectionChangedAction.Reset ? 0 : e.OldStartingIndex;

            this.HandleItemsRemoved(oldItems, oldStartingIndex);
            this.HandleItemsAdded(e.NewItems, e.NewStartingIndex);

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.backup.Clear();
            }
        }

        private void HandleItemsRemoved(IList items, int startingIndex)
        {
            if (items == null)
            {
                return;
            }

            for (int i = items.Count - 1; i >= 0; i--)
            {
                this.OnItemRemoved((T)items[i], startingIndex + i);
            }
        }

        private void OnItemRemoved(T item, int index)
        {
            this.SmartDetachItemPropertyChanged(item);
            this.RaiseEvent(this.ItemRemoved, item, index);
        }

        private void HandleItemsAdded(IList items, int startingIndex)
        {
            if (items == null)
            {
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                this.OnItemAdded((T)items[i], startingIndex + i);
            }
        }

        private void OnItemAdded(T item, int index)
        {
            this.SmartAttachItemPropertyChanged(item);
            this.RaiseEvent(this.ItemAdded, item, index);
        }

        private void RaiseEvent(EventHandler<ObservableItemCollectionChangedEventArgs<T>> handler, T item, int index)
        {
            if (handler != null)
            {
                ObservableItemCollectionChangedEventArgs<T> args = new ObservableItemCollectionChangedEventArgs<T>(item, index);
                handler(this, args);
            }
        }

        private void AttachItemsPropertyChanged()
        {
            foreach (T item in this)
            {
                this.SmartAttachItemPropertyChanged(item);
            }
        }

        private void DetachItemsPropertyChanged()
        {
            foreach (T item in this)
            {
                this.SmartDetachItemPropertyChanged(item);
            }
        }

        private void SmartAttachItemPropertyChanged(T item)
        {
            if (!this.hasItemPropertyChangedListeners)
            {
                return;
            }

            INotifyPropertyChanged observableItem = item as INotifyPropertyChanged;
            if (observableItem != null)
            {
                observableItem.PropertyChanged += this.OnItemPropertyChanged;
            }
        }

        private void SmartDetachItemPropertyChanged(T item)
        {
            if (!this.hasItemPropertyChangedListeners)
            {
                return;
            }

            INotifyPropertyChanged observableItem = item as INotifyPropertyChanged;
            if (observableItem != null)
            {
                observableItem.PropertyChanged -= this.OnItemPropertyChanged;
            }
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.itemPropertyChanged?.Invoke(this, new ItemPropertyChangedEventArgs<T>((T)sender, e.PropertyName));
        }
    }
}
