using Abc;
using System.Windows;

namespace WpfControls
{
    internal static class Utils
    {
        internal static Rect ToRect(AbcRect abcRect)
        {
            Rect rect = new Rect(abcRect.x, abcRect.y, abcRect.size.width, abcRect.size.height);
            return rect;
        }
    }
}