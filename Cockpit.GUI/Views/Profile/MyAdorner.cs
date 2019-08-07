using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Cockpit.GUI.Views.Profile
{
    public class MyAdorner : Adorner
    {
        private readonly int color;
        public MyAdorner(UIElement targetElement, int color) : base(targetElement)
        {
            this.color = color;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);
            switch(color)
            {
                case 1:
                    drawingContext.DrawRectangle(null, new Pen(Brushes.Red, 4), adornedElementRect);
                    break;
                case 2:
                    drawingContext.DrawRectangle(null, new Pen(Brushes.Green, 4), adornedElementRect);
                    break;
                default:
                    drawingContext.DrawRectangle(null, new Pen(Brushes.Orange, 4), adornedElementRect);
                    break;
            }
        }
    }
}
