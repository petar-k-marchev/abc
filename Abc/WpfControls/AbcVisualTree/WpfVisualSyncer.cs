using Abc.Visuals;
using System;
using System.Windows;

namespace WpfControls
{
    internal abstract class WpfVisualSyncer
    {
        internal readonly AbcVisual abcVisual;
        internal readonly UIElement nativeVisual;

        private bool isSyncing;

        protected WpfVisualSyncer(AbcVisual abcVisual, UIElement nativeVisual)
        {
            this.abcVisual = abcVisual;
            this.nativeVisual = nativeVisual;
        }

        internal bool IsSyncing
        {
            get
            {
                return this.isSyncing;
            }
            private set
            {
                if (this.isSyncing && value)
                {
                    throw new Exception(string.Format("You shouldn't need to call {0}() twice.", nameof(StartSync)));
                }

                this.isSyncing = value;
            }
        }

        internal virtual void StartSync()
        {
            this.IsSyncing = true;
        }

        internal virtual void StopSync()
        {
            this.IsSyncing = false;
        }
    }
}