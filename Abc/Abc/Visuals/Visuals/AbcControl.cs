using Abc.Primitives;
using System;

namespace Abc.Visuals
{
    internal abstract class AbcControl : IAbcControl
    {
        private IAbcVisual controlRoot;
        private NativeVisualTree visualTree;
        private bool isMeasurePhase;
        private AbcSize desiredMeasure;
        private bool isMeasureValid;
        private bool isArrangePhase;
        private AbcRect arrangeSlot;
        private bool isArrangeValid;
        private bool isPaintPhase;

        public IAbcVisual ControlRoot
        {
            get
            {
                return this.controlRoot;
            }
        }

        public NativeVisualTree VisualTree
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

                NativeVisualTree oldVisualTree = this.visualTree;
                this.visualTree = value;
                this.OnVisualTreeChanged(oldVisualTree);
            }
        }

        public AbcSize DesiredMeasure
        {
            get
            {
                return this.desiredMeasure;
            }
        }

        public AbcRect ArrangeSlot
        {
            get
            {
                return this.arrangeSlot;
            }
        }

        protected abstract AbcSize MeasureOverride(AbcMeasureContext context);

        protected abstract void ArrangeOverride(AbcArrangeContext context);

        protected abstract void PaintOverride(AbcContextBase context);

        protected virtual void OnVisualTreeChanged(NativeVisualTree oldVisualTree)
        {
            this.UpdateControlRootVisualTree();
        }

        private void UpdateControlRootVisualTree()
        {
            if (this.controlRoot != null)
            {
                this.controlRoot.VisualTree = this.VisualTree;
            }
        }

        public void Measure(AbcMeasureContext context)
        {
            if (this.isMeasureValid)
            {
                return;
            }

            this.isMeasurePhase = true;

            this.desiredMeasure = this.MeasureOverride(context);

            this.isMeasurePhase = false;
            this.isMeasureValid = true;
        }

        public void Arrange(AbcArrangeContext context)
        {
            if (this.isArrangeValid)
            {
                this.isArrangeValid = this.arrangeSlot == context.arrangeSlot;
            }

            if (this.isArrangeValid)
            {
                return;
            }

            this.isArrangePhase = true;
            this.arrangeSlot = context.arrangeSlot;
            this.ArrangeOverride(context);
            context.arrangeSlot = this.arrangeSlot;
            this.isArrangePhase = false;
            this.isArrangeValid = true;
        }

        public void Paint(AbcContextBase context)
        {
            this.isPaintPhase = true;
            this.PaintOverride(context);
            this.isPaintPhase = false;
        }

        internal void AddFlag(AbcVisualFlag flag)
        {
            if (this.isMeasureValid)
            {
                bool affectsMeasure = flag == AbcVisualFlag.AffectsMeasureOnly || flag == AbcVisualFlag.AffectsMeasureAndArrange;
            }
        }

        protected void SetControlRoot(IAbcVisual newControlRoot)
        {
            if (this.controlRoot != null)
            {
                this.controlRoot.VisualTree = null;
            }

            this.controlRoot = newControlRoot;

            this.UpdateControlRootVisualTree();
        }
    }
}
