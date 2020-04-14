using Abc;
using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls
{
    internal class WpfVisualTree : AbcVisualTree
    {
        internal static readonly AbcContextualPropertyKey SyncerPropertyKey = new AbcContextualPropertyKey();

        private static readonly Dictionary<Type, Func<AbcVisual, WpfVisualSyncer>> syncerCreator;

        private AbcVisual abcRoot;
        private UIElement nativeRoot;

        static WpfVisualTree()
        {
            syncerCreator = new Dictionary<Type, Func<AbcVisual, WpfVisualSyncer>>();
            syncerCreator[typeof(AbcLabel)] = CreateLabelSyncer;
            syncerCreator[typeof(AbcCanvas)] = CreateCanvasSyncer;
            syncerCreator[typeof(AbcRectangle)] = CreateRectangleSyncer;
        }

        internal AbcVisual AbcRoot
        {
            get
            {
                return this.abcRoot;
            }
            set
            {
                if (this.abcRoot == value)
                {
                    throw new Exception(string.Format("You shouldn't need to set the {0} twice.", nameof(AbcRoot)));
                }

                this.DiffuseRoots();
                this.abcRoot = value;
                this.abcRoot.VisualTree = this;
                this.FuseRoots();
            }
        }

        internal UIElement NativeRoot
        {
            get
            {
                return this.nativeRoot;
            }
            set
            {
                if (this.nativeRoot == value)
                {
                    throw new Exception(string.Format("You shouldn't need to set the {0} twice.", nameof(NativeRoot)));
                }

                this.DiffuseRoots();
                this.nativeRoot = value;
                this.FuseRoots();
            }
        }

        internal override void AttachToNativeParent(AbcVisual abcVisual)
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

        internal override void DetachFromNativeParent(AbcVisual abcVisual, AbcVisual oldParent)
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

        internal override void DetachFromVisualTree(AbcVisual abcVisual)
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

        internal override AbcSize Measure(AbcVisual abcVisual, AbcMeasureContext context)
        {
            WpfVisualSyncer visualSyncer = GetSyncer(abcVisual);
            UIElement nativeVisual = visualSyncer.nativeVisual;
            nativeVisual.Measure(new Size(context.availableSize.width, context.availableSize.height));
            return new AbcSize(nativeVisual.DesiredSize.Width, nativeVisual.DesiredSize.Height);
        }

        private static WpfVisualSyncer GetSyncer(AbcVisual abcVisual)
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

        private static void SetSyncer(AbcVisual abcVisual, WpfVisualSyncer syncer)
        {
            abcVisual.SetContextualPropertyValue(SyncerPropertyKey, new AbcContextualPropertyValue.AbcObject { value = syncer });
        }

        private static WpfVisualSyncer GetOrCreateSyncer(AbcVisual abcVisual)
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

        private static WpfVisualSyncer CreateSyncer(AbcVisual abcVisual)
        {
            Type abcVisualType = abcVisual.GetType();
            Func<AbcVisual, WpfVisualSyncer> creator;
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

        private static WpfVisualSyncer CreateLabelSyncer(AbcVisual abcVisual)
        {
            return new WpfLabelSyncer((AbcLabel)abcVisual);
        }

        private static WpfVisualSyncer CreateCanvasSyncer(AbcVisual abcVisual)
        {
            return new WpfCanvasSyncer((AbcCanvas)abcVisual);
        }

        private static WpfVisualSyncer CreateRectangleSyncer(AbcVisual abcVisual)
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
