namespace Abc.Visuals
{
    // is this class helpful? how?
    internal abstract class AbcContextualPropertyValue
    {
        internal class AbcInt : AbcContextualPropertyValue
        {
            internal int value;
        }

        internal class AbcString : AbcContextualPropertyValue
        {
            internal string value;
        }
    }
}
