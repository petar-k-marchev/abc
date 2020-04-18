using AbcDataVisualization;
using System.Windows;
using System.Windows.Controls;

namespace WpfControls.DataVisualization
{
    public class WpfNumericAxis : Control
    {
        private readonly AbcNumericAxis abcNumericAxis;
        private readonly WpfVisualTreeBase visualTree;

        static WpfNumericAxis()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfNumericAxis), new FrameworkPropertyMetadata(typeof(WpfNumericAxis)));
        }

        public WpfNumericAxis()
        {
            this.abcNumericAxis = new AbcNumericAxis();

            //this.visualTree = new WpfVisualTree();
            this.visualTree = new WpfDrawingVisualTree();

            this.visualTree.AbcRoot = this.abcNumericAxis;

            this.abcNumericAxis.UserMin = 0;
            this.abcNumericAxis.UserMax = 100;
            this.abcNumericAxis.UserStep = 25;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Canvas numericAxisCanvas = (Canvas)this.GetTemplateChild("PART_Canvas");
            this.visualTree.NativeRoot = numericAxisCanvas;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);

            this.abcNumericAxis.Measure(constraint.Width, constraint.Height);
            return new Size(this.abcNumericAxis.DesiredMeasure.width, this.abcNumericAxis.DesiredMeasure.height);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Size arrangedSize = base.ArrangeOverride(arrangeBounds);

            this.abcNumericAxis.Layout(0, 0, arrangeBounds.Width, arrangeBounds.Height);

            return arrangedSize;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == FontSizeProperty)
            {
                this.abcNumericAxis.FontSize = this.FontSize;
            }
        }
    }
}
