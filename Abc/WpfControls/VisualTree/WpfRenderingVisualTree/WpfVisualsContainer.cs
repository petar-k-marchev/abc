using Abc;
using Abc.Visuals;

namespace WpfControls.WpfRenderingVisualTreeInternals
{
    internal abstract class WpfVisualsContainer : WpfVisual, IAbcVisualsContainer
    {
        private readonly ObservableItemCollection<IAbcVisual> children;

        internal WpfVisualsContainer()
        {
            this.children = new ObservableItemCollection<IAbcVisual>();

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

        internal override void PaintOverride(AbcArrangeContext context)
        {
            base.PaintOverride(context);

            IAbcVisualsContainer abcVisualsContainer = this;
            foreach (IAbcVisual child in abcVisualsContainer.Children)
            {
                child.Paint(context);
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
