using System;
using System.Windows.Media;

namespace WpfControls
{
    internal interface IDrawingSurface
    {
        event EventHandler<DrawingContext> OnRender;
    }
}
