using Abc;
using Abc.Visuals;
using Android.Views;

namespace AndroidControls.VisualTree
{
    internal class AndroidVisualContainer : AndroidVisual, IAbcVisualsContainer
    {
        private readonly ObservableItemCollection<IAbcVisual> children;
        private readonly ViewGroup panel;

        internal AndroidVisualContainer(ViewGroup panel)
            : base(panel)
        {
            this.children = new ObservableItemCollection<IAbcVisual>();
            this.panel = panel;

            this.children.ItemAdded += this.Children_ItemAdded;
            this.children.ItemRemoved += this.Children_ItemRemoved;
        }

        ObservableItemCollection<IAbcVisual> IAbcVisualsContainer.Children
        {
            get
            {
                return this.children;
            }
        }

        private void Children_ItemAdded(object sender, ObservableItemCollectionChangedEventArgs<IAbcVisual> args)
        {
            args.Item.VisualParent = this;

            IAbcVisual abcVisual = this;
            if (abcVisual.VisualTree != null)
            {
                args.Item.VisualTree = abcVisual.VisualTree;
            }
        }

        private void Children_ItemRemoved(object sender, ObservableItemCollectionChangedEventArgs<IAbcVisual> args)
        {
            args.Item.VisualParent = null;

            //// Deliberately do not diconnect from VisualTree, so potential virtualization can be more efficient.
        }
    }
}
