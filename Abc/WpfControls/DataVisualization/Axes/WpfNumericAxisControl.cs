using Abc;
using Abc.Visuals;
using AbcDataVisualization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControls.DataVisualization
{
    public class WpfNumericAxisControl : Control
    {
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(WpfNumericAxisControl), new PropertyMetadata(0.0));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(WpfNumericAxisControl), new PropertyMetadata(100.0));

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
            "Step", typeof(double), typeof(WpfNumericAxisControl), new PropertyMetadata(25.0));

        private readonly WpfControlCoordinator<AbcNumericAxisControl> controlCoordinator;

        static WpfNumericAxisControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfNumericAxisControl), new FrameworkPropertyMetadata(typeof(WpfNumericAxisControl)));
        }

        public WpfNumericAxisControl()
        {
            string test = nameof(WpfRenderingVisualTree);
            
            AbcVisualTree visualTree = null;
            
            if (test == nameof(WpfVisualTree))
            {
                visualTree = new WpfVisualTree();
            }
            else if (test == nameof(WpfRenderingVisualTree))
            {
                visualTree = new WpfRenderingVisualTree();
            }

            this.controlCoordinator = new WpfControlCoordinator<AbcNumericAxisControl>(this, new AbcNumericAxisControl(), visualTree);
            this.controlCoordinator.abcControl.UserMin = this.Minimum;
            this.controlCoordinator.abcControl.UserMax = this.Maximum;
            this.controlCoordinator.abcControl.UserStep = this.Step;
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.controlCoordinator.visualTree is WpfVisualTree)
            {
                this.controlCoordinator.NativeControlRoot = (Canvas)this.GetTemplateChild("PART_Canvas");
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);

            AbcMeasureContext context = new AbcMeasureContext(new AbcSize(constraint.Width, constraint.Height));
            this.controlCoordinator.abcControl.Measure(context);
            return new Size(this.controlCoordinator.abcControl.DesiredMeasure.width, this.controlCoordinator.abcControl.DesiredMeasure.height);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Size arrangedSize = base.ArrangeOverride(arrangeBounds);

            AbcArrangeContext context = new AbcArrangeContext(new AbcRect(0, 0, arrangeBounds.Width, arrangeBounds.Height));
            this.controlCoordinator.abcControl.Arrange(context);

            return arrangedSize;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == FontSizeProperty)
            {
                if (this.controlCoordinator.visualTree is WpfVisualTree)
                {
                    //// font size will automatically propagate to textblock elements, so no need to 
                    //// set the abcNumericAxis.FontSize property
                    //// but we need to notify the axis that the measure is no longer valid
                    this.controlCoordinator.abcControl.InvalidateMeasure();
                }
                else
                {
                    this.controlCoordinator.abcControl.FontSize = this.FontSize;
                }
            }
            else if (e.Property == MinimumProperty)
            {
                this.controlCoordinator.abcControl.UserMin = this.Minimum;
            }
            else if (e.Property == MaximumProperty)
            {
                this.controlCoordinator.abcControl.UserMax = this.Maximum;
            }
            else if (e.Property == StepProperty)
            {
                this.controlCoordinator.abcControl.UserStep = this.Step;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.controlCoordinator.visualTree is WpfRenderingVisualTree)
            {
                AbcContextBase context = new AbcContextBase();
                context.Bag.SetBagObject(WpfRenderingVisualTree.DrawingContextIdentifier, drawingContext);
                this.controlCoordinator.abcControl.Paint(context);
            }
        }
    }
}
