﻿using Abc;
using Abc.Visuals;
using AbcDataVisualization;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidControls.VisualTree;

namespace AndroidControls.DataVisualization.Axes
{
    public class AndroidNumericAxisControl : ViewGroup
    {
        internal static Context AndroidContext;
        private double minimum;
        private double maximum;
        private double step;

        private readonly AndroidControlCoordinator<AbcNumericAxisControl> controlCoordinator;

        public AndroidNumericAxisControl(Context context)
            : base(context)
        {
            AndroidContext = context;

            AbcVisualTree visualTree = new AndroidVisualTree();

            this.controlCoordinator = new AndroidControlCoordinator<AbcNumericAxisControl>(this, new AbcNumericAxisControl() { FontSize = 18 }, visualTree);
            this.controlCoordinator.abcControl.UserMin = this.Minimum;
            this.controlCoordinator.abcControl.UserMax = this.Maximum;
            this.controlCoordinator.abcControl.UserStep = this.Step;
            this.controlCoordinator.NativeControlRoot = this;
        }

        public double Minimum
        {
            get
            {
                return this.minimum;
            }
            set
            {
                if (this.minimum != value)
                {
                    this.controlCoordinator.abcControl.UserMin = value;
                    this.minimum = value;
                }
            }
        }

        public double Maximum
        {
            get
            {
                return this.maximum;
            }
            set
            {
                if (this.maximum != value)
                {
                    this.controlCoordinator.abcControl.UserMax = value;
                    this.maximum = value;
                }
            }
        }

        public double Step
        {
            get
            {
                return this.step;
            }
            set
            {
                if (this.step != value)
                {
                    this.controlCoordinator.abcControl.UserStep = value;
                    this.step = value;
                }
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            AbcMeasureContext context = new AbcMeasureContext(new AbcSize(width, height));
            this.controlCoordinator.abcControl.Measure(context);

            this.SetMeasuredDimension((int)this.controlCoordinator.abcControl.DesiredMeasure.width, (int)this.controlCoordinator.abcControl.DesiredMeasure.height);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            AbcArrangeContext context = new AbcArrangeContext(new AbcRect(l, t, r - l, b - t));
            this.controlCoordinator.abcControl.Arrange(context);

        }
    }
}
