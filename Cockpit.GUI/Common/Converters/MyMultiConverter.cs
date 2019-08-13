using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Cockpit.GUI.Common.Converters
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double LeftOrTop, ZoomFactor;

            LeftOrTop = (double)values[0];
            ZoomFactor = (double)values[1];
            return LeftOrTop * ZoomFactor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MyMultiConverterMargin : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double left = 0, top = 0, right = 0, bottom = 0, width = 0, height = 0;

            left = (double)values[0];
            top = (double)values[1];
            right = (double)values[2];
            bottom = (double)values[3];
            width = (double)values[4];
            height = (double)values[5];

            Thickness margin = new Thickness
            {
                Left = (int)(left * width),
                Top = (int)(top * height),
                Right = (int)(right * width),
                Bottom = (int)(bottom * height)
            };

            return margin;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DrawGlyphes : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double width;
            double height;

            if (values.Length == 3)
            {
                width = (double)values[0];
                height = (double)values[1];
                double LineThickness = (double)values[2];
                Point Center = new Point(width / 2d, height / 2d);

                RotateTransform tr = new RotateTransform(225, Center.X, Center.Y);


                LineGeometry line = new LineGeometry(Center, new Point(Center.X, 0), tr);
                return line;
            }





            int glyph = (int)values[0];

            if (glyph == 0)
                return null;

            if (values.Length == 2 ) // Path.Fill Binding
            {
                if (glyph == 2 || glyph == 3)
                    return new SolidColorBrush((Color)values[1]);
                else
                    return null;
            }
            // Path.Data binding
            double glyphScale = (double)values[1];
            width = (double)values[2];
            height = (double)values[3];
            switch(glyph)
            {
                case 1:
                    Point Center = new Point(width / 2d, height / 2d);
                    double Radius = Math.Min(width, height) / 2d * glyphScale;
                    EllipseGeometry Circle = new EllipseGeometry(Center, Radius, Radius);
                    return Circle;
                case 2: // right arrow
                case 3: // left arrow
                    double glyphThickness = (double)values[4];
                    double y = height / 2d;
                    double arrowLength = width * glyphScale;
                    double padding = (width - arrowLength) / 2d;
                    double arrowLineLength = arrowLength * .6d;
                    double headHeightOffset = glyphThickness * 2d;

                    Point lineStart, lineEnd, head, head1, head2;

                    if (glyph == 2)
                    {
                        lineStart = new Point(padding, y);
                        lineEnd = new Point(lineStart.X + arrowLineLength, y);
                        head = new Point(width - padding, y);
                        head1 = new Point(lineEnd.X, y - headHeightOffset);
                        head2 = new Point(lineEnd.X, y + headHeightOffset);
                    }
                    else
                    {
                        lineStart = new Point(width - padding, y);
                        lineEnd = new Point(lineStart.X - arrowLineLength, y);
                        head = new Point(padding, y);
                        head1 = new Point(lineEnd.X, y + headHeightOffset);
                        head2 = new Point(lineEnd.X, y - headHeightOffset);
                    }

                    PathFigure arrow = new PathFigure();
                    arrow.IsClosed = false;
 
                    arrow.StartPoint = lineStart;
                    arrow.Segments.Add(new LineSegment(lineEnd, true));
                    arrow.Segments.Add(new LineSegment(head1, false));
                    arrow.Segments.Add(new LineSegment(head, false));
                    arrow.Segments.Add(new LineSegment(head2, false));
                    arrow.Segments.Add(new LineSegment(lineEnd, false));

                    PathGeometry geometry = new PathGeometry();
                    
                    geometry.Figures.Add(arrow);
                    return geometry;

                case 4: // upCaret
                case 5: // downCaret
                    double centerX = width / 2d;
                    double centerY = height / 2d;
                    double offsetX = centerX * glyphScale;
                    double offsetY = offsetX / 2d * (glyph == 4 ? 1 : -1);


                    PathFigure figure = new PathFigure();
                    figure.IsClosed = false;
                    figure.IsFilled = false;

                    figure.StartPoint = new Point(centerX - offsetX, centerY + offsetY);
                    figure.Segments.Add(new LineSegment(new Point(centerX, centerY - offsetY), true));
                    figure.Segments.Add(new LineSegment(new Point(centerX + offsetX, centerY + offsetY), true));

                    PathGeometry geometry2 = new PathGeometry();
                    geometry2.Figures.Add(figure);
                    return geometry2;


                default:
                    return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CircleScaling : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double width = 0, height = 0;
            double.TryParse(values[0].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out width);
            double.TryParse(values[1].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out height);
            //string p = parameter.ToString();

            //int[] param = p.Split(' ').Select(x => System.Convert.ToInt32(x, CultureInfo.InvariantCulture)).ToArray();

            Thickness margin = new Thickness
            {
                Left = width,
                Top = height,
                Right = 0,
                Bottom = 0
            };

            return margin;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
