using Abc.Primitives;
using Abc.Visuals;
using System;
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

        internal override bool IsMasterSlaveTypeOfVisualTree
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
            
            Panel nativeRoot = (Canvas)this.NativeRoot;
            MasterControlInfo masterControlInfo = (MasterControlInfo)this.AbcRoot.ControlInfo;
            WpfVisual abcRootVisual = (WpfVisual)masterControlInfo.slave;
            nativeRoot.Children.Add(abcRootVisual.uiElement);
        }

        private void DiffuseRoots()
        {
            if (this.AbcRoot == null ||
                this.NativeRoot == null)
            {
                return;
            }
            
            Panel nativeRoot = (Canvas)this.NativeRoot;
            MasterControlInfo masterControlInfo = (MasterControlInfo)this.AbcRoot.ControlInfo;
            WpfVisual abcRootVisual = (WpfVisual)masterControlInfo.slave;
            nativeRoot.Children.Remove(abcRootVisual.uiElement);
        }
    }
}
