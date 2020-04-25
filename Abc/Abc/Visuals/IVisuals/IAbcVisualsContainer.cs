using Abc.Miscellaneous;

namespace Abc.Visuals
{
    internal interface IAbcVisualsContainer : IAbcVisual
    {
        ObservableItemCollection<IAbcVisual> Children { get; }
    }
}
