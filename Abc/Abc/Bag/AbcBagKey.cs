using System.Threading;

namespace Abc.Visuals
{
    internal struct AbcBagKey
    {
        private static int givenBagKeyValues;

        internal readonly int Value;

        private AbcBagKey(int value)
        {
            this.Value = value;
        }

        public static bool operator ==(AbcBagKey abcBagKey1, AbcBagKey abcBagKey2)
        {
            return abcBagKey1.Value == abcBagKey2.Value;
        }

        public static bool operator !=(AbcBagKey abcBagKey1, AbcBagKey abcBagKey2)
        {
            return abcBagKey1.Value != abcBagKey2.Value;
        }

        public override int GetHashCode()
        {
            return this.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is AbcBagKey other)
            {
                return this == other;
            }

            return false;
        }

        internal static AbcBagKey GetNextBagKey()
        {
            int value = Interlocked.Increment(ref givenBagKeyValues);
            return new AbcBagKey(value);
        }
    }
}
