using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            if (abcVisual.VisualParent == null)
            {
                return;
            }

            WpfDrawCommandSyncer visualSyncer = GetOrCreateSyncer(abcVisual);
            visualSyncer.StartSync();
        }

        internal override void DetachFromNativeParent(AbcVisual abcVisual, AbcVisual oldParent)
        {
            WpfDrawCommandSyncer visualSyncer = GetSyncer(abcVisual);

            if (visualSyncer != null &&
                oldParent != null)
            {
                visualSyncer.StopSync();
            }
        }

        internal override void DetachFromVisualTree(AbcVisual abcVisual)
        {
        }

        internal override AbcSize Measure(AbcVisual abcVisual, AbcMeasureContext context)
        {
            WpfDrawCommandSyncer visualSyncer = GetSyncer(abcVisual);
            return visualSyncer.Measure(context);
        }

        internal static WpfDrawCommandSyncer GetSyncer(IAbcVisual abcVisual)
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

        internal void Render(DrawingContext drawingContext)
        {
            foreach (IAbcVisual child in ((IAbcCanvas)this.AbcRoot).Children)
            {
                WpfDrawCommandSyncer syncer = WpfDrawingVisualTree.GetSyncer(child);
                if (syncer is WpfLabelSyncer labelSyncer)
                {
                    labelSyncer.OnRender(drawingContext);
                }
            }
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
    }
}
