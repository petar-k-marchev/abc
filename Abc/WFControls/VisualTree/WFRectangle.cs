using Abc;
using Abc.Visuals;
using System.Drawing;
using System.Windows.Forms;

namespace WFControls.VisualTree
{
    internal class WFRectangle : WFVisual, IAbcRectangle
    {
        internal WFRectangle()
            : base(null)
        {
        }

        internal override void PaintOverride(AbcArrangeContext context)
        {
            base.PaintOverride(context);

            object paintEventArgsObject;
            context.Bag.TryGetBagObject(WFVisual.PaintEventArgsIdentifier, out paintEventArgsObject);
            PaintEventArgs paintEventArgs = (PaintEventArgs)paintEventArgsObject;
            IAbcRectangle abcRectangle = this;
            AbcRect slot = abcRectangle.ArrangeSlot;

            using (Pen pen = new Pen(Color.Black))
            {
                paintEventArgs.Graphics.DrawRectangle(pen, (float)slot.x, (float)slot.y, (float)slot.size.width, (float)slot.size.height);
            }
        }
    }
}
