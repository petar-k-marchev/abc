using System.Threading;

namespace Abc.Visuals
{
    internal class AbcContextualPropertyKey
    {
        private static int givenKeyValues;

        internal readonly int key = GetNextKey();

        protected static int GetNextKey()
        {
            int nextKey = Interlocked.Increment(ref givenKeyValues);
            return nextKey;
        }
    }
}
