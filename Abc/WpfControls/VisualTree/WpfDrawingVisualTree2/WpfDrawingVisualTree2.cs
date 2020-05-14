using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Collections.Generic;

namespace WpfControls
{
    internal class WpfDrawingVisualTree2 : WpfVisualTreeBase
    {
        internal static readonly string DrawingContextIdentifier = "System.Windows.Media.DrawingContext";

        private static readonly Dictionary<Type, Func<IAbcVisual>> abcVisualCreator;

        static WpfDrawingVisualTree2()
        {
            abcVisualCreator = new Dictionary<Type, Func<IAbcVisual>>();
            abcVisualCreator[typeof(IAbcLabel)] = CreateLabel;
            abcVisualCreator[typeof(IAbcCanvas)] = CreateCanvas;
            abcVisualCreator[typeof(IAbcRectangle)] = CreateRectangle;
        }

        internal override bool IsMasterSlaveTypeOfVisualTree
        {
            get { return false; }
        }

        internal override void AttachToNativeParent(IAbcVisual abcVisual)
        {
        }

        internal override void DetachFromNativeParent(IAbcVisual abcVisual, IAbcVisual oldParent)
        {
        }

        internal override void DetachFromVisualTree(IAbcVisual abcVisual)
        {
        }

        internal override AbcSize Measure(IAbcVisual abcVisual, AbcMeasureContext context)
        {
            throw new Exception();
        }

        internal override IAbcVisual CreateVisual(Type type)
        {
            Func<IAbcVisual> creator = abcVisualCreator[type];
            IAbcVisual visual = creator();
            return visual;
        }

        private static IAbcVisual CreateLabel()
        {
            return new WpfDrawLabelVisual();
        }

        private static IAbcVisual CreateCanvas()
        {
            return new WpfDrawCanvasVisual();
        }

        private static IAbcVisual CreateRectangle()
        {
            return new WpfDrawRectangleVisual();
        }
    }
}
