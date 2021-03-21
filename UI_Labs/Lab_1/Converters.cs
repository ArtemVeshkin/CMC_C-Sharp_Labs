using System.Numerics;
using System.Windows.Data;
using System;
using ClassLibrary;

namespace Lab_1
{
    [ValueConversion(typeof(Vector2), typeof(string))]
    public class CoordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Vector2 vector2 = (Vector2)value;
            return $"Координаты: ({vector2.X:f3}, {vector2.Y:f3})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    [ValueConversion(typeof(double), typeof(string))]
    public class ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double valueDouble = (double)value;
            return $"Значение: {valueDouble:f4}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    [ValueConversion(typeof(Grid1D), typeof(string))]
    public class XGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Grid1D grid = (Grid1D)value;
            return $"Сетка по X: Шаг = {grid.Step}, Число шагов = {grid.NSteps}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    [ValueConversion(typeof(Grid1D), typeof(string))]
    public class YGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Grid1D grid = (Grid1D)value;
            return $"Сетка по Y: Шаг = {grid.Step}, Число шагов = {grid.NSteps}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
