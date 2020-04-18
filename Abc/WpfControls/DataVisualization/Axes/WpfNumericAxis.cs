using Abc.Primitives;
using Abc.Visuals;
using AbcDataVisualization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfControls.WpfDrawingVisualTreeInternals;

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
            this.visualTree.NativeRoot = this;

            this.visualTree.AbcRoot = this.abcNumericAxis;

            this.abcNumericAxis.UserMin = 0;
            this.abcNumericAxis.UserMax = 100;
            this.abcNumericAxis.UserStep = 25;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Canvas numericAxisCanvas = (Canvas)this.GetTemplateChild("PART_Canvas");
            //this.visualTree.NativeRoot = numericAxisCanvas;
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

            //AbcLayoutContext context = new AbcLayoutContext(new AbcRect(0, 0, arrangeBounds.Width, arrangeBounds.Height));
            //this.abcNumericAxis.Layout(context);

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

            // can draw here directly instead of in visual tree

            AbcLayoutContext context = new AbcLayoutContext(new AbcRect(0, 0, this.ActualWidth, this.ActualHeight));
            this.abcNumericAxis.Layout(context);

            // we must render these manually or optimization in engine will not call Layout
            // seems like Layout needs to be separated - Arrange and Render ?

            foreach (var child in this.abcNumericAxis.children)
            {
                WpfDrawCommandSyncer syncer = WpfDrawingVisualTree.GetSyncer(child);
                if (syncer is WpfLabelSyncer labelSyncer)
                {
                    labelSyncer.OnRender(drawingContext);
                }
            }

        }
    }
}
