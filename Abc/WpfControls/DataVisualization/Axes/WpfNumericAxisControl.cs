using Abc.Primitives;
using Abc.Visuals;
using AbcDataVisualization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControls.DataVisualization
{
    public class WpfNumericAxisControl : Control
    {
        private readonly AbcNumericAxisControl abcNumericAxis;
        private readonly WpfVisualTreeBase visualTree;

        static WpfNumericAxisControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfNumericAxisControl), new FrameworkPropertyMetadata(typeof(WpfNumericAxisControl)));
        }

        public WpfNumericAxisControl()
        {
            // WpfVisualTree
            // WpfDrawingVisualTree
            // WpfVisualTree2
            // WpfDrawingVisualTree2
            string test = nameof(WpfDrawingVisualTree2);

            if (test == nameof(WpfVisualTree))
            {
                this.visualTree = new WpfVisualTree();
            }
            else if (test == nameof(WpfDrawingVisualTree))
            {
                this.visualTree = new WpfDrawingVisualTree();
                this.visualTree.NativeRoot = this;
            }
            else if (test == nameof(WpfVisualTree2))
            {
                this.visualTree = new WpfVisualTree2();
            }
            else if (test == nameof(WpfDrawingVisualTree2))
            {
                this.visualTree = new WpfDrawingVisualTree2();
            }

            this.abcNumericAxis = new AbcNumericAxisControl();
            this.abcNumericAxis.VisualTree = this.visualTree;
            this.visualTree.AbcRoot = this.abcNumericAxis.ControlRoot;

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

            if (this.visualTree is WpfVisualTree2)
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

            AbcArrangeContext context = new AbcArrangeContext(new AbcRect(0, 0, arrangeBounds.Width, arrangeBounds.Height));
            this.abcNumericAxis.Arrange(context);

            return arrangedSize;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (this.visualTree is WpfVisualTree)
            {
                // font size propagates automatically to visuals
                // we can skip notifying axis (for testing purposes mainly)
                // now we must expect for visual-tree-part to let the engine know there is a measure invalidated
            }
            else if (this.visualTree is WpfVisualTree2)
            {
                // font size propagates automatically to visuals
                // we can skip notifying axis (for testing purposes mainly)
                // now we must expect for visual-tree-part to let the engine know there is a measure invalidated
            }
            else
            {
                if (e.Property == FontSizeProperty)
                {
                    this.abcNumericAxis.FontSize = this.FontSize;
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            AbcArrangeContext context = new AbcArrangeContext(new AbcRect(0, 0, this.ActualWidth, this.ActualHeight));
            context.Bag.SetBagObject(WpfDrawingVisualTree2.DrawingContextIdentifier, drawingContext);
            this.abcNumericAxis.Paint(context);

            if (this.visualTree is WpfDrawingVisualTree wpfDrawingVisualTree)
            {
                wpfDrawingVisualTree.Render(drawingContext);
            }
        }
    }
}
