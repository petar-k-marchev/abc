namespace Abc.Visuals
{
    internal static class AbcMath
    {
        internal static bool IsValidDouble(double value)
        {
            return double.MinValue < value && value < double.MaxValue;
        }
    }
}
