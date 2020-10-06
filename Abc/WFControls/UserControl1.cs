using Abc.Visuals;
using System.Windows.Forms;
using WFControls.DataVisualization.Axes;
using WFControls.VisualTree;

namespace WFControls
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            WFLabel wfLabel = new WFLabel();
            IAbcLabel abcLabel = wfLabel;
            abcLabel.Text = "kor";
            this.Controls.Add(wfLabel.control);

            this.Controls.Add(new WFNumericAxisControl() { Size = new System.Drawing.Size(200, 200) });
        }
    }
}
