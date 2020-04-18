using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControls
{
    public class WpfDrawingCanvas : Canvas, IDrawingSurface
    {
        private event EventHandler<DrawingContext> onRender;

        event EventHandler<DrawingContext> IDrawingSurface.OnRender
        {
            add { this.onRender += value; }
            remove { this.onRender -= value; }
        }

        // OnRender is declared UIElement
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            this.onRender?.Invoke(this, dc);
        }
    }
}
