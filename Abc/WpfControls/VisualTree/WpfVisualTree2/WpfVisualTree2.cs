using Abc.Primitives;
using Abc.Visuals;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfControls.WpfVisualTreeInternals;

namespace WpfControls
{
    internal class WpfVisualTree2 : WpfVisualTreeBase
    {
        internal WpfVisualTree2()
        {
            this.visualCreator[typeof(IAbcLabel)] = CreateLabel;
            this.visualCreator[typeof(IAbcRectangle)] = CreateRectangle;
            this.visualCreator[typeof(IAbcCanvas)] = CreateCanvas;
        }

        internal override bool IsAsd
        {
            get { return true; }
        }

        internal override void AttachToNativeParent(IAbcVisual abcVisual)
        {
            throw new Exception();
        }

        internal override void DetachFromNativeParent(IAbcVisual abcVisual, IAbcVisual oldParent)
        {
            throw new Exception();
        }

        internal override void DetachFromVisualTree(IAbcVisual abcVisual)
        {
            throw new Exception();
        }

        internal override AbcSize Measure(IAbcVisual abcVisual, AbcMeasureContext context)
        {
            throw new Exception();
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

        private static WpfVisual CreateLabel()
        {
            return new WpfLabel();
        }

        private static WpfVisual CreateCanvas()
        {
            return new WpfCanvas();
        }

        private static WpfVisual CreateRectangle()
        {
            return new WpfRectangle();
        }

        private WpfCanvas wpfCanvas;

        private void FuseRoots()
        {
            if (this.AbcRoot == null ||
                this.NativeRoot == null)
            {
                return;
            }

            //WpfVisualSyncer visualSyncer = GetOrCreateSyncer(this.AbcRoot);
            //AddVisualToParent(visualSyncer.nativeVisual, this.NativeRoot);
        }

        private void DiffuseRoots()
        {
            //if (this.AbcRoot == null ||
            //    this.NativeRoot == null)
            //{
            //    return;
            //}

            //WpfVisualSyncer visualSyncer = GetSyncer(this.AbcRoot);
            //if (visualSyncer != null)
            //{
            //    RemoveVisualFromParent(visualSyncer.nativeVisual, this.NativeRoot);
            //}
        }
    }
}
