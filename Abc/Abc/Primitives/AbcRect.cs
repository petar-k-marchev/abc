using Abc.Miscellaneous;

namespace Abc.Primitives
{
    internal struct AbcRect
    {
        internal double x;
        internal double y;
        internal AbcSize size;

        internal AbcRect(double x, double y, AbcSize size)
        {
            this.x = x;
            this.y = y;
            this.size = size;
        }

        internal AbcRect(double x, double y, double width, double height)
        {
            this.x = x;
            this.y = y;
            this.size = new AbcSize(width, height);
        }

        internal AbcRect(AbcSize size)
        {
            this.x = 0;
            this.y = 0;
            this.size = size;
        }

        internal AbcRect(double width, double height)
        {
            this.x = 0;
            this.y = 0;
            this.size = new AbcSize(width, height);
        }

        internal double Right()
        {
            return this.x + this.size.width;
        }

        internal double Bottom()
        {
            return this.y + this.size.height;
        }

        internal bool IsValid()
        {
            return this.size.IsValid();
        }

        public static bool operator ==(AbcRect rect1, AbcRect rect2)
        {
            return rect1.x == rect2.x &&
                rect1.y == rect2.y &&
                rect1.size == rect2.size;
        }

        public static bool operator !=(AbcRect rect1, AbcRect rect2)
        {
            return !(rect1 == rect2);
        }

        public override int GetHashCode()
        {
            return HashHelper.CombineHashCodes(this.x.GetHashCode(), this.y.GetHashCode(), this.size.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj is AbcRect other)
            {
                return this == other;
            }

            return false;
        }

    }
}
