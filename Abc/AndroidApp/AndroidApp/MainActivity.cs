using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AndroidControls.DataVisualization.Axes;

namespace AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private AndroidNumericAxisControl axis;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            this.axis = new AndroidNumericAxisControl(this.BaseContext);
            this.axis.Minimum = 0;
            this.axis.Maximum = 100;
            this.axis.Step = 25;

            var layout = new LinearLayout(this.BaseContext);
            layout.Orientation = Orientation.Vertical;
            layout.AddView(axis);

            var changeMinBtn = new Button(this.BaseContext);
            changeMinBtn.Click += this.ChangeMinBtnClk;
            changeMinBtn.Text = "Change min";
            layout.AddView(changeMinBtn);

            var changeMaxBtn = new Button(this.BaseContext);
            changeMaxBtn.Click += this.ChangeMaxBtnClk;
            changeMaxBtn.Text = "Change max";
            layout.AddView(changeMaxBtn);

            var changeStepBtn = new Button(this.BaseContext);
            changeStepBtn.Click += this.ChangeStepBtnClk;
            changeStepBtn.Text = "Change step";
            layout.AddView(changeStepBtn);

            SetContentView(layout);
        }

        private void ChangeMinBtnClk(object sender, System.EventArgs e)
        {
            this.axis.Minimum = -10;
        }

        private void ChangeMaxBtnClk(object sender, System.EventArgs e)
        {
            this.axis.Maximum = 200;
        }

        private void ChangeStepBtnClk(object sender, System.EventArgs e)
        {
            this.axis.Step = 50;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

