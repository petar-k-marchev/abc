using Abc.Visuals;
using System;

namespace Abc.Controls
{
    internal abstract partial class AbcControl : IAbcControl
    {
        private event EventHandler<InvalidationRequestEventArgs> invalidationRequest;

        private AbcVisualTree visualTree;
        private IAbcVisual root;

        internal AbcVisualTree VisualTree
        {
            get
            {
                return this.visualTree;
            }
            set
            {
                if (this.visualTree == value)
                {
                    throw new Exception(string.Format("You shouldn't need to set the {0} twice.", nameof(VisualTree)));
                }

                AbcVisualTree oldVisualTree = this.visualTree;
                this.visualTree = value;
                this.OnVisualTreeChanged(oldVisualTree);
            }
        }

        internal IAbcVisual Root
        {
            get { return this.root; }
        }

        protected void SetRoot(IAbcVisual newRoot)
        {
            this.root = newRoot;
        }

        protected virtual void OnVisualTreeChanged(AbcVisualTree oldVisualTree)
        {
        }
    }
}
