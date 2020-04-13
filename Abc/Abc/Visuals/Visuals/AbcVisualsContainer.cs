using Abc.Miscellaneous;

namespace Abc.Visuals
{
    internal abstract class AbcVisualsContainer : AbcVisual
    {
        internal readonly ObservableItemCollection<AbcVisual> children;

        internal AbcVisualsContainer()
        {
            this.children = new ObservableItemCollection<AbcVisual>();
            this.children.ItemAdded += this.Children_ItemAdded;
            this.children.ItemRemoved += this.Children_ItemRemoved;
        }

        private void Children_ItemAdded(object sender, ObservableItemCollectionChangedEventArgs<AbcVisual> e)
        {
            e.Item.VisualParent = this;
        }

        private void Children_ItemRemoved(object sender, ObservableItemCollectionChangedEventArgs<AbcVisual> e)
        {
            e.Item.VisualParent = null;
        }

        protected override void OnVisualTreeChanged(AbcVisualTree oldVisualTree)
        {
            base.OnVisualTreeChanged(oldVisualTree);

            foreach (AbcVisual child in this.children)
            {
                child.VisualTree = this.VisualTree;
            }
        }
    }
}
