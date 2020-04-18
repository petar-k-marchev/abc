using Abc;
using System;
using System.Windows;

namespace WpfControls
{
    internal abstract class WpfVisualTreeBase : NativeVisualTree
    {
        private UIElement nativeRoot;

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

                this.OnNativeRootChanging(value);
            }
        }

        internal virtual void OnNativeRootChanging(UIElement newNativeRoot)
        {
            this.nativeRoot = newNativeRoot;
        }
    }
}
