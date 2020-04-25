using Abc.Primitives;
using Abc.Visuals;
using AbcDataVisualization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            if (false)
            {
                this.visualTree = new WpfVisualTree();
            }
            else
            {
                this.visualTree = new WpfDrawingVisualTree();
                this.visualTree.NativeRoot = this;
            }

            this.visualTree.AbcRoot = this.abcNumericAxis;

            this.abcNumericAxis.UserMin = 0;
            this.abcNumericAxis.UserMax = 100;
            this.abcNumericAxis.UserStep = 25;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.visualTree is WpfVisualTree)
            {
                Canvas numericAxisCanvas = (Canvas)this.GetTemplateChild("PART_Canvas");
                this.visualTree.NativeRoot = numericAxisCanvas;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);

            AbcMeasureContext context = new AbcMeasureContext(new AbcSize(constraint.Width, constraint.Height));
            this.abcNumericAxis.Measure(context);
            return new Size(this.abcNumericAxis.DesiredMeasure.width, this.abcNumericAxis.DesiredMeasure.height);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Size arrangedSize = base.ArrangeOverride(arrangeBounds);

            if (this.visualTree is WpfVisualTree)
            {
                AbcArrangeContext context = new AbcArrangeContext(new AbcRect(0, 0, arrangeBounds.Width, arrangeBounds.Height));
                this.abcNumericAxis.Arrange(context);
            }

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

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.visualTree is WpfDrawingVisualTree wpfDrawingVisualTree)
            {
                AbcArrangeContext context = new AbcArrangeContext(new AbcRect(0, 0, this.ActualWidth, this.ActualHeight));
                this.abcNumericAxis.Arrange(context);
                wpfDrawingVisualTree.Render(drawingContext);
            }
        }
    }
}
