using Abc.Visuals;
using System.Drawing;
using System.Windows.Forms;

namespace WFControls.VisualTree
{
    internal class WFLabel : WFVisual, IAbcLabel
    {
        private readonly Label label;

        public WFLabel()
            : base(new Label())
        {
            this.label = (Label)this.control;
        }

        string IAbcLabel.Text
        {
            get
            {
                return this.label.Text;
            }
            set
            {
                this.label.Text = value;
            }
        }

        double IAbcLabel.FontSize
        {
            get
            {
                return this.label.Font.Size;
            }
            set
            {
                float floatValue = (float)value;

                if (floatValue > 0)
                {
                    this.label.Font = new System.Drawing.Font(this.label.Font.FontFamily, floatValue, FontStyle.Regular);
                }
                else
                {
                    this.label.Font = new System.Drawing.Font(this.label.Font, FontStyle.Regular);
                }
            }
        }
    }
}
