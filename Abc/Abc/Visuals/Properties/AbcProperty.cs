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

    internal class AbcProperty<T>
    {
        private readonly AbcVisual abcVisual;
        private readonly AbcVisualFlag visualFlag;

        private T value;

        internal AbcProperty(AbcVisual abcVisual, AbcVisualFlag visualFlag)
        {
            this.abcVisual = abcVisual;
            this.visualFlag = visualFlag;
        }

        internal event EventHandler<AbcPropertyChangedEventArgs> Changed;

        internal T Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (!object.Equals(this.value, value))
                {
                    T oldValue = this.value;
                    this.value = value;
                    this.Changed?.Invoke(this, new AbcPropertyChangedEventArgs(oldValue, value));
                    this.abcVisual.AddFlag(this.visualFlag);
                }
            }
        }

        internal class AbcPropertyChangedEventArgs : EventArgs
        {
            private T oldValue;
            private T newValue;

            public AbcPropertyChangedEventArgs(T oldValue, T newValue)
            {
                this.oldValue = oldValue;
                this.newValue = newValue;
            }
        }
    }
}
