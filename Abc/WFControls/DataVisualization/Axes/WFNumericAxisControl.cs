using Abc;
using Abc.Visuals;
using AbcDataVisualization;
using System.Drawing;
using System.Windows.Forms;
using WFControls.VisualTree;

namespace WFControls.DataVisualization.Axes
{
    public class WFNumericAxisControl : Control
    {
        private readonly WFControlCoordinator<AbcNumericAxisControl> controlCoordinator;

        public WFNumericAxisControl()
        {
            string test = nameof(WFVisualTree);

            AbcVisualTree visualTree = null;

            if (test == nameof(WFVisualTree))
            {
                visualTree = new WFVisualTree();
            }

            this.controlCoordinator = new WFControlCoordinator<AbcNumericAxisControl>(this, new AbcNumericAxisControl(), visualTree);
            this.controlCoordinator.abcControl.UserMin = this.Minimum;
            this.controlCoordinator.abcControl.UserMax = this.Maximum;
            this.controlCoordinator.abcControl.UserStep = this.Step;
            this.controlCoordinator.NativeControlRoot = new Panel();
            this.Controls.Add(this.controlCoordinator.NativeControlRoot);

            this.Minimum = 0;
            this.Maximum = 100;
            this.Step = 25;
        }

        public double Minimum
        {
            get { return this.controlCoordinator.abcControl.UserMin; }
            set { this.controlCoordinator.abcControl.UserMin = value; }
        }

        public double Maximum
        {
            get { return this.controlCoordinator.abcControl.UserMax; }
            set { this.controlCoordinator.abcControl.UserMax = value; }
        }

        public double Step
        {
            get { return this.controlCoordinator.abcControl.UserStep; }
            set { this.controlCoordinator.abcControl.UserStep = value; }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            AbcMeasureContext context = new AbcMeasureContext(new AbcSize(proposedSize.Width, proposedSize.Height));
            this.controlCoordinator.abcControl.Measure(context);
            return new Size((int)this.controlCoordinator.abcControl.DesiredMeasure.width, (int)this.controlCoordinator.abcControl.DesiredMeasure.height);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            Rectangle arrangeBounds = this.Bounds;
            AbcArrangeContext context = new AbcArrangeContext(new AbcRect(0, 0, arrangeBounds.Width, arrangeBounds.Height));
            this.controlCoordinator.abcControl.Arrange(context);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            AbcArrangeContext context = new AbcArrangeContext(new AbcRect(0, 0, this.Bounds.Width, this.Bounds.Height));
            context.Bag.SetBagObject(WFVisual.PaintEventArgsIdentifier, e);
            this.controlCoordinator.abcControl.Paint(context);
        }
    }
}
