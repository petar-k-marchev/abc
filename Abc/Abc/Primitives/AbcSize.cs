namespace Abc.Visuals
{
    internal struct AbcSize
    {
        internal double width;
        internal double height;

        internal AbcSize(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        internal bool IsValid()
        {
            return 0 < this.width && this.width < double.MaxValue &&
                0 < this.height && this.height < double.MaxValue;
        }
    }
}
