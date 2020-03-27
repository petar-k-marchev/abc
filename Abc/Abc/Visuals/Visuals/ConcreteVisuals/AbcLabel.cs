namespace Abc.Visuals
{
    internal class AbcLabel : AbcVisual
    {
        private string text;

        internal string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.AddFlag(AbcVisualFlag.AffectsMeasureAndLayout);
                }
            }
        }
    }
}
