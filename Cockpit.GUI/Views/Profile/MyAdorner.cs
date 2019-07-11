using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Cockpit.GUI.Views.Profile
{
    public class MyAdorner : Adorner
    {
        private readonly bool first;
        public MyAdorner(UIElement targetElement, bool first) : base(targetElement)
        {
            this.first = first;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            if (first)
                drawingContext.DrawRectangle(null, new Pen(Brushes.Orange, 4), adornedElementRect);
            else
                drawingContext.DrawRectangle(null, new Pen(Brushes.Green, 4), adornedElementRect);
        }
    }
}
