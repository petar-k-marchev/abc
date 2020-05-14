using Abc.Primitives;
using Abc.Visuals;
using System.Windows;
using System.Windows.Media;

namespace WpfControls
{
    internal class WpfDrawRectangleVisual : WpfDrawInstructionVisual, IAbcRectangle
    {
        internal override AbcSize MeasureOverride(AbcMeasureContext context)
        {
            return new AbcSize(0, 0);
        }

        internal override void ArrangeOverride(AbcArrangeContext context)
        {
        }

        internal override void PaintOverride(AbcContextBase context)
        {
            IAbcVisual abcVisual = this;
            AbcRect arrangeSlot = abcVisual.ArrangeSlot;

            object drawingContextObject;
            context.Bag.TryGetBagObject(WpfDrawingVisualTree2.DrawingContextIdentifier, out drawingContextObject);
            DrawingContext drawingContext = (DrawingContext)drawingContextObject;

            //Point position = new Point(arrangeSlot.x, arrangeSlot.y);
            //Pen pen = new Pen();
            drawingContext.DrawRectangle(Brushes.Black, null, new Rect(arrangeSlot.x, arrangeSlot.y, arrangeSlot.size.width, arrangeSlot.size.height));
        }
    }
}
