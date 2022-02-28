using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace cpXamarin_APP.Convertors
{
    public class czConvertPriceLevelToColor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                1 => Color.FromRgb(125, 25, 25),//Color.FromRgb(100, 20, 20), //Color.Brown,
                2 => Color.Black,
                3 => Color.Green,
                4 => Color.Orange,
                5 => Color.Red,
                6 => Color.Blue,//Color.BlueViolet
                7 => Color.Magenta,

                _ => Color.Silver
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


    public class czConvertNaklsItemIndexesToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                0 => Color.Red,
                1 => Color.Black,
                2 => Color.Gray,
                _ => Color.Red
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }





    public class czConvertBoolToColor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return Color.Azure;
            return value switch
            {
                false=> (parameter as Array).GetValue(0),
                true => (parameter as Array).GetValue(1),
                _ => Color.Aquamarine
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


    public class czConvertZeroToColor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return (parameter as Array).GetValue(0);

            if (parameter == null) return Color.Azure;

            
            
            double d = System.Convert.ToDouble(value);
            if (d == 0) return (parameter as Array).GetValue(0);
            else return (parameter as Array).GetValue(1);

            //return value switch
            //{
            //    0.0 => (parameter as Array).GetValue(0),
            //    0 => (parameter as Array).GetValue(0),
            //    _ => (parameter as Array).GetValue(1)
            //};
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


}
