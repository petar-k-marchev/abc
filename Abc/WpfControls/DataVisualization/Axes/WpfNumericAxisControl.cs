using Abc;
using Abc.Visuals;
using AbcDataVisualization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfControls.WpfVisualTreeInternals;

namespace WpfControls.DataVisualization
{
    public class WpfNumericAxisControl : Control
    {
        private readonly AbcNumericAxisControl abcNumericAxis;
        private readonly AbcVisualTree visualTree;
        private Canvas numericAxisCanvas;

        static WpfNumericAxisControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfNumericAxisControl), new FrameworkPropertyMetadata(typeof(WpfNumericAxisControl)));
        }

        public WpfNumericAxisControl()
        {
            string test = nameof(WpfRenderingVisualTree);

            if (test == nameof(WpfVisualTree))
            {
                this.visualTree = new WpfVisualTree();
            }
            else if (test == nameof(WpfRenderingVisualTree))
            {
                this.visualTree = new WpfRenderingVisualTree();
            }

            this.abcNumericAxis = new AbcNumericAxisControl();
            this.abcNumericAxis.VisualTree = this.visualTree;
            this.abcNumericAxis.UserMin = 0;
            this.abcNumericAxis.UserMax = 100;
            this.abcNumericAxis.UserStep = 25;
        }

        private Canvas NumericAxisCanvas
        {
            get
            {
                return this.numericAxisCanvas;
            }
            set
            {
                if (this.numericAxisCanvas != value)
                {
                    if (this.numericAxisCanvas != null)
                    {
                        WpfVisual wpfVisual = (WpfVisual)this.abcNumericAxis.Root;
                        this.numericAxisCanvas.Children.Remove(wpfVisual.uiElement);
                    }

                    this.numericAxisCanvas = value;

                    if (this.numericAxisCanvas != null)
                    {
                        WpfVisual wpfVisual = (WpfVisual)this.abcNumericAxis.Root;
                        this.numericAxisCanvas.Children.Add(wpfVisual.uiElement);
                    }
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.visualTree is WpfVisualTree)
            {
                this.NumericAxisCanvas = (Canvas)this.GetTemplateChild("PART_Canvas");
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

            if (this.visualTree is WpfRenderingVisualTree)
            {
                AbcArrangeContext context = new AbcArrangeContext(new AbcRect(0, 0, this.ActualWidth, this.ActualHeight));
                context.Bag.SetBagObject(WpfRenderingVisualTree.DrawingContextIdentifier, drawingContext);
                this.abcNumericAxis.Paint(context);
            }
        }
    }
}
