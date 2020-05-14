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
    }
}
