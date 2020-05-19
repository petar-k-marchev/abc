using Abc.Visuals;
using AndroidControls.DataVisualization.Axes;

namespace AndroidControls.VisualTree
{
    internal class AndroidLabel : AndroidVisual, IAbcLabel
    {
        private readonly AndroidTextView textView;

        public AndroidLabel()
            : base(new AndroidTextView(AndroidNumericAxisControl.AndroidContext))
        {
            this.textView = (AndroidTextView)this.view;
        }

        string IAbcLabel.Text
        {
            get
            {
                return this.textView.Text;
            }
            set
            {
                this.textView.Text = value;
            }
        }

        double IAbcLabel.FontSize
        {
            get
            {
                return this.textView.TextSize;
            }
            set
            {
                if (value > 0)
                {
                    this.textView.TextSize = (float)value;
                }
                else
                {
                    this.textView.TextSize = 0;
                }

                this.InvalidateMeasure();
            }
        }
    }
}
