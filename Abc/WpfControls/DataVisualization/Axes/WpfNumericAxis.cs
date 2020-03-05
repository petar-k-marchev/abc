using System.Windows;
using System.Windows.Controls;

namespace WpfControls.DataVisualization
{
    public class WpfNumericAxis : Control
    {
        private WpfVisualTree visualTree;

        static WpfNumericAxis()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfNumericAxis), new FrameworkPropertyMetadata(typeof(WpfNumericAxis)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Canvas numericAxisCanvas = (Canvas)this.GetTemplateChild("PART_Canvas");
            //this.visualTree
            //how do you tie these two together
        }
    }
}
