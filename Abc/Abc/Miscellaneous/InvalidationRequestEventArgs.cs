namespace Abc
{
    internal struct InvalidationRequestEventArgs
    {
        internal readonly InvalidationRequestFlag flag;

        internal InvalidationRequestEventArgs(InvalidationRequestFlag flag)
        {
            this.flag = flag;
        }
    }
}
