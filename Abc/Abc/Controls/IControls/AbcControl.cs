using Abc.Visuals;
using System;

namespace Abc.Controls
{
    internal abstract partial class AbcControl : IAbcControl
    {
        private AbcVisualTree visualTree;

        public AbcVisualTree VisualTree
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

        public IAbcVisual Root
        {
            get;
            protected set;
        }

        protected virtual void OnVisualTreeChanged(AbcVisualTree oldVisualTree)
        {
        }
    }
}
