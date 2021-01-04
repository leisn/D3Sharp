using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace D3Sharp.Demo.Helps
{
    public class ControlXy
    {
        public FrameworkElement Control { get; set; }
        public string Target { get; set; }
    }

    public class CenterXyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (double)value;
            var xy = (ControlXy)parameter;
            double width = 0;
            if (xy.Target == "X")
                width = xy.Control.ActualWidth;
            else if (xy.Target == "Y")
                width = xy.Control.ActualHeight;
            else if (xy.Target == "Width")
                width = xy.Control.Width;
            else if (xy.Target == "Height")
                width = xy.Control.Height;
            else return val;
            return val - width / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
