namespace Abc
{
    internal static class HashHelper
    {
        internal static int CombineHashCodes(int hashCode1, int hashCode2)
        {
            int rocade3 = (hashCode1 << 3) | (hashCode1 >> 29);
            int rocade21 = (hashCode2 << 21) | (hashCode2 >> 11);
            return (rocade3 + hashCode1) ^ (rocade21 + hashCode2);
        }

        internal static int CombineHashCodes(int hashCode1, int hashCode2, int hashCode3)
        {
            int rocade5 = (hashCode1 << 5) | (hashCode1 >> 27);
            int rocade13 = (hashCode2 << 13) | (hashCode2 >> 19);
            int rocade17 = (hashCode3 << 17) | (hashCode3 >> 15);
            return (rocade5 + hashCode1) ^ (rocade13 - hashCode2) ^ (rocade17 + hashCode3);
        }

        internal static int CombineHashCodes(double[] array)
        {
            int hashCode = 1806464371;
            int length = array.Length;

            for (int i = 0; i < length; i += 2)
            {
                int remainingValues = length - i;
                if (remainingValues > 1)
                {
                    hashCode = CombineHashCodes(hashCode, array[i].GetHashCode(), array[i + 1].GetHashCode());
                }
                else
                {
                    hashCode = CombineHashCodes(hashCode, array[i].GetHashCode());
                }
            }

            return hashCode;
        }
    }
}