using System;

namespace Abc
{
    internal static class AbcMath
    {
        internal static bool IsValidDouble(double value)
        {
            return double.MinValue < value && value < double.MaxValue;
        }

        internal static bool IsValidPositiveDouble(double value)
        {
            return 0 < value && value < double.MaxValue;
        }

        internal static bool IsValidNonNegativeDouble(double value)
        {
            return 0 <= value && value < double.MaxValue;
        }

        internal static int Round(double value)
        {
            if (value >= 0)
            {
                return (int)(value + 0.5);
            }
            else
            {
                return (int)(value - 0.5);
            }
        }
    }
}
