namespace Abc.Visuals
{
    // is this class helpful? how?
    internal abstract class AbcContextualPropertyValue
    {
        internal class AbcObject : AbcContextualPropertyValue
        {
            internal object value;
        }

        internal class AbcInt : AbcContextualPropertyValue
        {
            internal int value;
        }

        internal class AbcString : AbcContextualPropertyValue
        {
            internal string value;
        }

        internal class AbcRect : AbcContextualPropertyValue
        {
            internal Abc.Primitives.AbcRect value;
        }
    }
}
