using System.Windows;
using System.Windows.Controls;

namespace WpfControls.WpfVisualTreeInternals
{
    internal class SlotPanel : Panel
    {
        internal static readonly Rect invalidSlot = new Rect(double.MinValue, double.MinValue, 0, 0);

        internal static readonly DependencyProperty SlotProperty = DependencyProperty.RegisterAttached(
            "Slot", typeof(Rect), typeof(SlotPanel), new PropertyMetadata(invalidSlot));

        internal static Rect GetSlot(DependencyObject obj)
        {
            return (Rect)obj.GetValue(SlotProperty);
        }

        internal static void SetSlot(DependencyObject obj, Rect value)
        {
            obj.SetValue(SlotProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size arrangedSize = base.ArrangeOverride(finalSize);

            Rect bounds = new Rect(0, 0, finalSize.Width, finalSize.Height);

            foreach (UIElement child in this.Children)
            {
                Rect slot = GetSlot(child);
                Rect actualSlot = slot == invalidSlot ? bounds : slot;
                child.Arrange(actualSlot);
            }

            return arrangedSize;
        }
    }
}