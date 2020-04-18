using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfControls.WpfDrawingVisualTreeInternals;

namespace WpfControls
{
    internal class WpfDrawingVisualTree : WpfVisualTreeBase
    {
        private static readonly AbcContextualPropertyKey SyncerPropertyKey = new AbcContextualPropertyKey();
        private static readonly Dictionary<Type, Func<AbcVisual, WpfDrawCommandSyncer>> syncerCreator;

        static WpfDrawingVisualTree()
        {
            syncerCreator = new Dictionary<Type, Func<AbcVisual, WpfDrawCommandSyncer>>();
            syncerCreator[typeof(AbcLabel)] = CreateLabelSyncer;
            syncerCreator[typeof(AbcCanvas)] = CreateCanvasSyncer;
            syncerCreator[typeof(AbcRectangle)] = CreateRectangleSyncer;
        }

        internal override void AttachToNativeParent(AbcVisual abcVisual)
        {
        }

        internal override void DetachFromNativeParent(AbcVisual abcVisual, AbcVisual oldParent)
        {
        }

        internal override void DetachFromVisualTree(AbcVisual abcVisual)
        {
        }

        internal override AbcSize Measure(AbcVisual abcVisual, AbcMeasureContext context)
        {
            WpfDrawCommandSyncer visualSyncer = GetOrCreateSyncer(abcVisual);
            return visualSyncer.Measure(context);
        }

        internal override void OnNativeRootChanging(UIElement newNativeRoot)
        {
            IDrawingSurface oldDrawingSurface = (IDrawingSurface)this.NativeRoot;
            IDrawingSurface newDrawingSurface = (IDrawingSurface)newNativeRoot;

            base.OnNativeRootChanging((UIElement)newDrawingSurface);

            if (oldDrawingSurface != null)
            {
                oldDrawingSurface.OnRender -= this.DrawingSurface_OnRender;
            }

            if (newDrawingSurface != null)
            {
                newDrawingSurface.OnRender += this.DrawingSurface_OnRender;
            }
        }

        private static WpfDrawCommandSyncer GetSyncer(AbcVisual abcVisual)
        {
            if (abcVisual == null)
            {
                return null;
            }

            AbcContextualPropertyValue syncerPropertyValue = abcVisual.GetContextualPropertyValue(SyncerPropertyKey);
            object syncerObject = ((AbcContextualPropertyValue.AbcObject)syncerPropertyValue)?.value;
            WpfDrawCommandSyncer syncer = (WpfDrawCommandSyncer)syncerObject;
            return syncer;
        }

        private static void SetSyncer(AbcVisual abcVisual, WpfDrawCommandSyncer syncer)
        {
            abcVisual.SetContextualPropertyValue(SyncerPropertyKey, new AbcContextualPropertyValue.AbcObject { value = syncer });
        }

        private static WpfDrawCommandSyncer GetOrCreateSyncer(AbcVisual abcVisual)
        {
            WpfDrawCommandSyncer syncer = GetSyncer(abcVisual);
            if (syncer == null)
            {
                syncer = CreateSyncer(abcVisual);
                SetSyncer(abcVisual, syncer);
            }
            return syncer;
        }

        private static WpfDrawCommandSyncer CreateSyncer(AbcVisual abcVisual)
        {
            Type abcVisualType = abcVisual.GetType();
            Func<AbcVisual, WpfDrawCommandSyncer> creator;
            if (syncerCreator.TryGetValue(abcVisualType, out creator))
            {
                WpfDrawCommandSyncer syncer = creator(abcVisual);
                return syncer;
            }

            foreach (var pair in syncerCreator)
            {
                if (abcVisualType.IsSubclassOf(pair.Key))
                {
                    WpfDrawCommandSyncer syncer = pair.Value(abcVisual);
                    return syncer;
                }
            }

            throw new Exception();
        }

        private static WpfDrawCommandSyncer CreateLabelSyncer(AbcVisual abcVisual)
        {
            return new WpfLabelSyncer((AbcLabel)abcVisual);
        }

        private static WpfDrawCommandSyncer CreateCanvasSyncer(AbcVisual abcVisual)
        {
            return new WpfCanvasSyncer((AbcCanvas)abcVisual);
        }

        private static WpfDrawCommandSyncer CreateRectangleSyncer(AbcVisual abcVisual)
        {
            return new WpfRectangleSyncer((AbcRectangle)abcVisual);
        }

        private void DrawingSurface_OnRender(object sender, System.Windows.Media.DrawingContext dc)
        {
            FrameworkElement frameworkElement = (FrameworkElement)sender;
            this.AbcRoot?.Layout(0, 0, frameworkElement.ActualWidth, frameworkElement.ActualHeight);

            var axis = (AbcDataVisualization.AbcNumericAxis)this.AbcRoot;
            foreach (var child in axis.children)
            {
                WpfDrawCommandSyncer syncer = GetSyncer(child);
                if (syncer is WpfLabelSyncer labelSyncer)
                {
                    labelSyncer.OnRender(dc);
                }
            }

            // foreach the abc visuals? and render via syncer?
            // or inherit each visual and render (in Layout)
            // or else
        }
    }
}
