using System;

namespace Abc.Visuals
{
    internal class AbcProperty
    {
        internal class Double : AbcProperty
        {
            private double value;

            internal event EventHandler Changed;

            internal double Value
            {
                get
                {
                    return this.value;
                }
                set
                {
                    if (this.value != value)
                    {
                        this.value = value;
                        this.Changed?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        internal class DoubleWithDefault : Double
        {
            private readonly double defaultValue;

            private bool isDefault;
            private bool isSettingDefault;

            internal DoubleWithDefault(double defaultValue)
            {
                this.defaultValue = defaultValue;
                this.Value = defaultValue;
                this.Changed += this.OnValueChanged;
                this.isDefault = true;
            }

            internal bool IsDefault
            {
                get
                {
                    return this.isDefault;
                }
            }

            internal void SetDefault()
            {
                this.isSettingDefault = true;
                this.Value = this.defaultValue;
                this.isSettingDefault = false;
            }

            private void OnValueChanged(object sender, EventArgs e)
            {
                this.isDefault = this.isSettingDefault;
            }
        }
    }
}
