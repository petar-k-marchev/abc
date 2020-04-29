using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfControls.WpfVisualTreeInternals;

namespace WpfControls
{
    internal class WpfVisualTree : WpfVisualTreeBase
    {
        private static readonly AbcContextualPropertyKey SyncerPropertyKey = new AbcContextualPropertyKey();
        private static readonly Dictionary<Type, Func<IAbcVisual, WpfVisualSyncer>> syncerCreator;

        static WpfVisualTree()
        {
            syncerCreator = new Dictionary<Type, Func<IAbcVisual, WpfVisualSyncer>>();
            syncerCreator[typeof(AbcLabel)] = CreateLabelSyncer;
            syncerCreator[typeof(AbcCanvas)] = CreateCanvasSyncer;
            syncerCreator[typeof(AbcRectangle)] = CreateRectangleSyncer;
        }

        internal override bool IsAsd
        {
            get { return false; }
        }

        internal override void AttachToNativeParent(IAbcVisual abcVisual)
        {
            if (abcVisual.VisualParent == null)
            {
                return;
            }

            WpfVisualSyncer visualSyncer = GetOrCreateSyncer(abcVisual);
            WpfVisualSyncer parentSyncer = GetOrCreateSyncer(abcVisual.VisualParent);

            visualSyncer.StartSync();
            AddVisualToParent(visualSyncer.nativeVisual, parentSyncer.nativeVisual);
        }

        internal override void DetachFromNativeParent(IAbcVisual abcVisual, IAbcVisual oldParent)
        {
            WpfVisualSyncer visualSyncer = GetSyncer(abcVisual);
            WpfVisualSyncer oldParentSyncer = GetSyncer(oldParent);

            if (visualSyncer != null &&
                oldParentSyncer != null)
            {
                visualSyncer.StopSync();
                RemoveVisualFromParent(visualSyncer.nativeVisual, oldParentSyncer.nativeVisual);
            }
        }

        internal override void DetachFromVisualTree(IAbcVisual abcVisual)
        {
            WpfVisualSyncer visualSyncer = GetSyncer(abcVisual);
            if (visualSyncer == null)
            {
                return;
            }

            WpfVisualSyncer parentSyncer = GetSyncer(abcVisual.VisualParent);
            if (parentSyncer != null)
            {
                visualSyncer.StopSync();
                RemoveVisualFromParent(visualSyncer.nativeVisual, parentSyncer.nativeVisual);
            }

            SetSyncer(abcVisual, null);
        }

        internal override AbcSize Measure(IAbcVisual abcVisual, AbcMeasureContext context)
        {
            WpfVisualSyncer visualSyncer = GetSyncer(abcVisual);
            return visualSyncer.Measure(context);
        }

        internal override void OnAbcRootChanging(IAbcVisual value)
        {
            this.DiffuseRoots();
            base.OnAbcRootChanging(value);
            this.FuseRoots();
        }

        internal override void OnNativeRootChanging(UIElement newNativeRoot)
        {
            this.DiffuseRoots();
            base.OnNativeRootChanging(newNativeRoot);
            this.FuseRoots();
        }

        private static WpfVisualSyncer GetSyncer(IAbcVisual abcVisual)
        {
            if (abcVisual == null)
            {
                return null;
            }

            AbcContextualPropertyValue syncerPropertyValue = abcVisual.GetContextualPropertyValue(SyncerPropertyKey);
            object syncerObject = ((AbcContextualPropertyValue.AbcObject)syncerPropertyValue)?.value;
            WpfVisualSyncer syncer = (WpfVisualSyncer)syncerObject;
            return syncer;
        }

        private static void SetSyncer(IAbcVisual abcVisual, WpfVisualSyncer syncer)
        {
            abcVisual.SetContextualPropertyValue(SyncerPropertyKey, new AbcContextualPropertyValue.AbcObject { value = syncer });
        }

        private static WpfVisualSyncer GetOrCreateSyncer(IAbcVisual abcVisual)
        {
            WpfVisualSyncer syncer = GetSyncer(abcVisual);
            if (syncer == null)
            {
                syncer = CreateSyncer(abcVisual);
                SetSyncer(abcVisual, syncer);
            }
            return syncer;
        }

        private static void AddVisualToParent(UIElement visual, UIElement parent)
        {
            Panel panel = (Panel)parent;
            panel.Children.Add(visual);
        }

        private static void RemoveVisualFromParent(UIElement visual, UIElement parent)
        {
            Panel oldPanel = (Panel)parent;
            oldPanel.Children.Remove(visual);
        }

        private static WpfVisualSyncer CreateSyncer(IAbcVisual abcVisual)
        {
            Type abcVisualType = abcVisual.GetType();
            Func<IAbcVisual, WpfVisualSyncer> creator;
            if (syncerCreator.TryGetValue(abcVisualType, out creator))
            {
                WpfVisualSyncer syncer = creator(abcVisual);
                return syncer;
            }

            foreach (var pair in syncerCreator)
            {
                if (abcVisualType.IsSubclassOf(pair.Key))
                {
                    WpfVisualSyncer syncer = pair.Value(abcVisual);
                    return syncer;
                }
            }

            throw new Exception();
        }

        private static WpfVisualSyncer CreateLabelSyncer(IAbcVisual abcVisual)
        {
            return new WpfLabelSyncer((AbcLabel)abcVisual);
        }

        private static WpfVisualSyncer CreateCanvasSyncer(IAbcVisual abcVisual)
        {
            return new WpfCanvasSyncer((AbcCanvas)abcVisual);
        }

        private static WpfVisualSyncer CreateRectangleSyncer(IAbcVisual abcVisual)
        {
            return new WpfRectangleSyncer((AbcRectangle)abcVisual);
        }

        private void FuseRoots()
        {
            if (this.AbcRoot == null ||
                this.NativeRoot == null)
            {
                return;
            }

            WpfVisualSyncer visualSyncer = GetOrCreateSyncer(this.AbcRoot);
            AddVisualToParent(visualSyncer.nativeVisual, this.NativeRoot);
        }

        private void DiffuseRoots()
        {
            if (this.AbcRoot == null ||
                this.NativeRoot == null)
            {
                return;
            }

            WpfVisualSyncer visualSyncer = GetSyncer(this.AbcRoot);
            if (visualSyncer != null)
            {
                RemoveVisualFromParent(visualSyncer.nativeVisual, this.NativeRoot);
            }
        }
    }
}
