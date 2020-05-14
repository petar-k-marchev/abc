using Abc.Miscellaneous;
using Abc.Visuals;

namespace WpfControls
{
    internal abstract class WpfDrawVisualsContainerVisual : WpfDrawInstructionVisual, IAbcVisualsContainer
    {
        ObservableItemCollection<IAbcVisual> IAbcVisualsContainer.Children { get; } = new ObservableItemCollection<IAbcVisual>();

        internal override void PaintOverride(AbcContextBase context)
        {
            IAbcVisualsContainer abcVisualsContainer = this;
            foreach (IAbcVisual child in abcVisualsContainer.Children)
            {
                child.Paint(context);
            }
        }
    }
}
