using Abc.Miscellaneous;

namespace Abc.Visuals
{
    internal abstract class AbcVisualsContainer : AbcVisual, IAbcVisualsContainer
    {
        internal AbcVisualsContainer()
        {
            this.Children = new ObservableItemCollection<IAbcVisual>();
            this.Children.ItemAdded += this.Children_ItemAdded;
            this.Children.ItemRemoved += this.Children_ItemRemoved;
        }

        public ObservableItemCollection<IAbcVisual> Children { get; }

        private void Children_ItemAdded(object sender, ObservableItemCollectionChangedEventArgs<IAbcVisual> e)
        {
            e.Item.VisualParent = this;

            if (this.VisualTree != null)
            {
                e.Item.VisualTree = this.VisualTree;
            }
        }

        private void Children_ItemRemoved(object sender, ObservableItemCollectionChangedEventArgs<IAbcVisual> e)
        {
            e.Item.VisualParent = null;

            // Deliberately do not diconnect from VisualTree, so virtualization can be more efficient.
        }

        protected override void OnVisualTreeChanged(NativeVisualTree oldVisualTree)
        {
            base.OnVisualTreeChanged(oldVisualTree);

            foreach (AbcVisual child in this.Children)
            {
                child.VisualTree = this.VisualTree;
            }
        }
    }
}
