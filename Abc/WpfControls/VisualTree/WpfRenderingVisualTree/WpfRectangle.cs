using Abc;
using Abc.Visuals;
using System.Windows;
using System.Windows.Media;

namespace WpfControls.WpfRenderingVisualTreeInternals
{
    internal class WpfRectangle : WpfVisual, IAbcRectangle
    {
        internal override void PaintOverride(AbcContextBase context)
        {
            IAbcVisual abcVisual = this;
            AbcRect arrangeSlot = abcVisual.ArrangeSlot;

            object drawingContextObject;
            context.Bag.TryGetBagObject(WpfRenderingVisualTree.DrawingContextIdentifier, out drawingContextObject);
            DrawingContext drawingContext = (DrawingContext)drawingContextObject;

            drawingContext.DrawRectangle(Brushes.Black, null, new Rect(arrangeSlot.x, arrangeSlot.y, arrangeSlot.size.width, arrangeSlot.size.height));
        }
    }
}
