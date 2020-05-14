using Abc;
using Abc.Visuals;
using System.Windows.Controls;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class WpfVisualsContainer : WpfVisual, IAbcVisualsContainer
    {
        private readonly ObservableItemCollection<IAbcVisual> children;
        private readonly Panel panel;

        internal WpfVisualsContainer(Panel panel)
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