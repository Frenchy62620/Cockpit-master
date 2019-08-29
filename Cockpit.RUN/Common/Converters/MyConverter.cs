using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Cockpit.RUN.Common.Converters
{
    //public class MyConverterAdorner : IValueConverter
    //{
        // parameter = "coefdiviseur sizetoretract"  "2 10"
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        Point p = new Point();
    //        p.X = (double)((Point)value).X;
    //        p.Y = (double)((Point)value).Y;

    //        return  p;




    //        //if (targetType == typeof(double))
    //        //{
    //        //    double dvalue = 0;
    //        //    if (!double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out dvalue))
    //        //        return 0d;

    //        //    double[] param = parameter.ToString().Split(' ').Select(x => System.Convert.ToDouble(x, CultureInfo.InvariantCulture)).ToArray();

    //        //    return dvalue / param[0] - param[1];
    //        //}

    //        //if (targetType == typeof(bool))
    //        //    return !(bool)value;

    //        //return value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    //public class MyConverterSize : IValueConverter
    //{
    //    // parameter = "coefdiviseur sizetoretract"  "2 10"
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        //if (parameter != null && parameter.ToString().Equals("transform"))
    //        //    return (double)value;


    //        if (targetType == typeof(double))
    //        {
    //            double dvalue = 0;
    //            if (!double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out dvalue))
    //                return 0d;

    //            double[] param = parameter.ToString().Split(' ').Select(x => System.Convert.ToDouble(x, CultureInfo.InvariantCulture)).ToArray();

    //            return dvalue / param[0] - param[1];
    //        }

    //        if (targetType == typeof(bool))
    //            return !(bool)value;

    //        return value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class MyConverterMargin : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        bool isSelected = (bool)value;
    //        Thickness margin = new Thickness(isSelected ? 8 : 0);
    //        return margin;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class FormatNumber : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (parameter != null && parameter.Equals("ToDouble"))
    //        {
    //            double v;
    //            var b = double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out v);
    //            if (b == false) v = 0d;
    //            return v;
    //        }

    //        if (value == null)
    //            return string.Empty;

    //        Int32.TryParse(parameter.ToString(), out int param);
    //        return $"{value.ToString().PadLeft(param, '0')}";
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return value;
    //    }
    //}

    //public class MyConverterStyle : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (targetType != typeof(Style))
    //        {
    //            throw new InvalidOperationException("The target must be a Style");
    //        }

    //        string[] param = parameter.ToString().Split(' ');

    //        var styleProperty = parameter as string;
    //        if (value == null || styleProperty == null)
    //        {
    //            return null;
    //        }

    //        string styleValue = value.GetType()
    //            .GetProperty(styleProperty)
    //            .GetValue(value, null)
    //            .ToString();
    //        if (styleValue == null)
    //        {
    //            return null;
    //        }

    //        Style newStyle = (Style)Application.Current.TryFindResource(styleValue);
    //        return newStyle;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class MyconverterColorToSolidColorBrush : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        try
    //        {
    //            Color c = (Color)value;
    //            return new SolidColorBrush(c);
    //        }
    //        catch
    //        {
    //            return new SolidColorBrush(Color.FromRgb(0, 0, 0));
    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //[ValueConversion(typeof(Point[]), typeof(Geometry))]
    //public class PointsToPathConverter : IValueConverter
    //{

    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        Point[] points = (Point[])value;
    //        if (points.Length > 0)
    //        {
    //            Point start = points[0];
    //            List<LineSegment> segments = new List<LineSegment>();
    //            for (int i = 1; i < points.Length; i++)
    //            {
    //                segments.Add(new LineSegment(points[i], true));
    //            }
    //            PathFigure figure = new PathFigure(start, segments, false); //true if closed
    //            PathGeometry geometry = new PathGeometry();
    //            geometry.Figures.Add(figure);
    //            return geometry;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotSupportedException();
    //    }

    //}

    [ValueConversion(typeof(string), typeof(Point))]
    public class MyConverterRender : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double[] param = value.ToString().Split(',').Select(x => System.Convert.ToDouble(x, CultureInfo.InvariantCulture)).ToArray();
            return new Point(param[0], param[1]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
