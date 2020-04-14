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
        internal static readonly AbcContextualPropertyKey NativeVisualWrapPropertyKey = new AbcContextualPropertyKey();

        private static readonly Dictionary<Type, Func<AbcVisual, NativeVisualWrap>> abcVisualTypeToNativeWrap;

        private AbcVisual abcRoot;
        private UIElement nativeRoot;

        static WpfVisualTree()
        {
            abcVisualTypeToNativeWrap = new Dictionary<Type, Func<AbcVisual, NativeVisualWrap>>();
            abcVisualTypeToNativeWrap[typeof(AbcLabel)] = CreateLabelWrap;
            abcVisualTypeToNativeWrap[typeof(AbcCanvas)] = CreateCanvasWrap;
            abcVisualTypeToNativeWrap[typeof(AbcRectangle)] = CreateRectangleWrap;
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

        internal override void AttachToNativeParent(AbcVisual abcVisual, AbcVisual oldVisualParent)
        {
            NativeVisualWrap visualWrap = GetOrCreateWrap(abcVisual);
            NativeVisualWrap oldParentWrap = GetWrap(oldVisualParent);

            if (oldParentWrap?.nativeVisual != null)
            {
                RemoveVisualFromParent(visualWrap.nativeVisual, oldParentWrap.nativeVisual);
            }

            if (abcVisual.VisualParent == null)
            {
                return;
            }

            NativeVisualWrap parentWrap = GetOrCreateWrap(abcVisual.VisualParent);
            AddVisualToParent(visualWrap.nativeVisual, parentWrap.nativeVisual);
        }

        internal override AbcSize Measure(AbcVisual abcVisual, AbcMeasureContext context)
        {
            NativeVisualWrap visualWrap = GetWrap(abcVisual);
            UIElement nativeVisual = (UIElement)visualWrap.nativeVisual;
            nativeVisual.Measure(new Size(context.availableSize.width, context.availableSize.height));
            return new AbcSize(nativeVisual.DesiredSize.Width, nativeVisual.DesiredSize.Height);
        }

        private static NativeVisualWrap GetWrap(AbcVisual abcVisual)
        {
            if (abcVisual == null)
            {
                return null;
            }

            AbcContextualPropertyValue nativeVisualWrapPropertyValue = abcVisual.GetContextualPropertyValue(NativeVisualWrapPropertyKey);
            object nativeVisualObject = ((AbcContextualPropertyValue.AbcObject)nativeVisualWrapPropertyValue)?.value;
            NativeVisualWrap nativeVisualWrap = (NativeVisualWrap)nativeVisualObject;
            return nativeVisualWrap;
        }

        private static NativeVisualWrap GetOrCreateWrap(AbcVisual abcVisual)
        {
            NativeVisualWrap nativeVisualWrap = GetWrap(abcVisual);
            if (nativeVisualWrap == null)
            {
                nativeVisualWrap = CreateWrap(abcVisual);                
                abcVisual.SetContextualPropertyValue(NativeVisualWrapPropertyKey, new AbcContextualPropertyValue.AbcObject { value = nativeVisualWrap });
            }
            return nativeVisualWrap;
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

        private static NativeVisualWrap CreateWrap(AbcVisual abcVisual)
        {
            Type type = abcVisual.GetType();
            Func<AbcVisual, NativeVisualWrap> func;
            if (abcVisualTypeToNativeWrap.TryGetValue(type, out func))
            {
                NativeVisualWrap wrap = func(abcVisual);
                return wrap;
            }

            foreach (var pair in abcVisualTypeToNativeWrap)
            {
                if (type.IsSubclassOf(pair.Key))
                {
                    NativeVisualWrap wrap = pair.Value(abcVisual);
                    return wrap;
                }
            }

            throw new Exception();
        }

        private static NativeVisualWrap CreateLabelWrap(AbcVisual abcVisual)
        {
            WpfVisualSyncer syncer = new WpfLabelSyncer((AbcLabel)abcVisual);
            return new NativeVisualWrap { nativeVisual = syncer.nativeVisual, syncer = syncer };
        }

        private static NativeVisualWrap CreateCanvasWrap(AbcVisual abcVisual)
        {
            WpfVisualSyncer syncer = new WpfCanvasSyncer((AbcCanvas)abcVisual);
            return new NativeVisualWrap { nativeVisual = syncer.nativeVisual, syncer = syncer };
        }

        private static NativeVisualWrap CreateRectangleWrap(AbcVisual abcVisual)
        {
            WpfVisualSyncer syncer = new WpfRectangleSyncer((AbcRectangle)abcVisual);
            return new NativeVisualWrap { nativeVisual = syncer.nativeVisual, syncer = syncer };
        }

        private void FuseRoots()
        {
            if (this.AbcRoot == null ||
                this.NativeRoot == null)
            {
                return;
            }

            NativeVisualWrap visualWrap = GetOrCreateWrap(this.AbcRoot);
            AddVisualToParent(visualWrap.nativeVisual, this.NativeRoot);
        }

        private void DiffuseRoots()
        {
            if (this.NativeRoot == null)
            {
                return;
            }
            
            NativeVisualWrap visualWrap = GetWrap(this.AbcRoot);
            if (visualWrap != null)
            {
                RemoveVisualFromParent(visualWrap.nativeVisual, this.NativeRoot);
            }
        }

        class NativeVisualWrap
        {
            internal UIElement nativeVisual;
            internal WpfVisualSyncer syncer;
        }
    }
}
