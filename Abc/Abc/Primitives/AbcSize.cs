using Abc.Miscellaneous;

namespace Abc.Primitives
{
    internal struct AbcSize
    {
        internal static readonly AbcSize Invalid = new AbcSize(-1, -1);

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

        public static bool operator ==(AbcSize size1, AbcSize size2)
        {
            return size1.width == size2.width &&
                size1.height == size2.height;
        }

        public static bool operator !=(AbcSize size1, AbcSize size2)
        {
            return !(size1 == size2);
        }

        public override int GetHashCode()
        {
            return HashHelper.CombineHashCodes(this.width.GetHashCode(), this.height.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj is AbcSize other)
            {
                return this == other;
            }

            return false;
        }

    }
}
