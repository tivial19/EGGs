using cpADD.APP;
using cpADD.EXT;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cpXamarin_APP.Convertors
{

    public class czConvertor_MarkupExtension : IMarkupExtension
    {
        //protected Binding cvBinding;

        public object ProvideValue(IServiceProvider sp)
        {
            return this;
        }
    }






    public class czConvertBoolToBoolInvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

    }



    
    public class czConvertZeroToBool : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                0.0 => false,
                0 => false,
                _ => true
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }






    //public class czConvertDecimalToString : czConvertor_MarkupExtension, IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null) return null;
    //        return ((decimal)value).ToSF(System.Convert.ToInt32(parameter));
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;
    //        decimal.TryParse(value?.ToString() ?? "", out decimal cxR);
    //        return cxR;
    //    }
    //}




    //public class czConvertDecimalToString : czConvertor_MarkupExtension, IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null) return null;
    //        else
    //        {
    //            int cxDigits = 0;
    //            if (parameter != null) cxDigits = System.Convert.ToInt32(parameter);

    //            string cxS = new string('#', cxDigits);
    //            return ((decimal)value).ToString("0." + cxS, CultureInfo.InvariantCulture);//always dot
    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (string.IsNullOrWhiteSpace(value?.ToString() ?? ""))
    //        {
    //            if (targetType == typeof(decimal?)) return null;
    //            else return 0;
    //        }
    //        else
    //        {
    //            string cxS = (value?.ToString() ?? "").Replace(",", ".");//replays to dot
    //            if (decimal.TryParse(cxS, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal cxR)) return cxR;
    //            else throw new czAPP_Exception_Convertor("test");
    //        }
    //    }

    //}



    public class czConvertDecimalToString_Simple : czConvertor_MarkupExtension, IValueConverter
    {
        int cvDigits = 2;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            else
            {
                if (parameter != null) cvDigits = System.Convert.ToInt32(parameter);
                string cxS = new string('#', cvDigits);
                return ((decimal)value).ToString("0." + cxS, CultureInfo.InvariantCulture);//always dot
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString() ?? ""))
            {
                if (targetType == typeof(decimal?)) return null;
                else return 0;
            }
            else
            {
                string cxS = (value?.ToString() ?? "").Replace(",", ".");//replays to dot
                if (decimal.TryParse(cxS, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal cxR)) return cxR;
                return 0;
            }
        }

    }




    public class czConvertDecimalToString : czConvertor_MarkupExtension, IValueConverter
    {
        decimal cvLast_Value;
        int cvDigits = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            else
            {
                cvLast_Value = ((decimal)value);
                if (parameter != null) cvDigits = System.Convert.ToInt32(parameter);
                string cxS = new string('#', cvDigits);
                return cvLast_Value.ToString("0." + cxS, CultureInfo.InvariantCulture);//always dot
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString() ?? ""))
            {
                if (targetType == typeof(decimal?)) return null;
                else return 0;
            }
            else
            {
                string cxS = (value?.ToString() ?? "").Replace(",", ".");//replays to dot
                if (decimal.TryParse(cxS, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal cxR))
                    return cxR;
                else return cvLast_Value;
            }
        }

    }









    public class czConvertDecimalToString_Multi : IMultiValueConverter, IMarkupExtension
    {
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (value == null) return null;
        //    return ((decimal)value).ToSF(System.Convert.ToInt32(parameter));
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (targetType == typeof(decimal?))
        //    {

        //    }
        //    if (value == null) return null;
        //    decimal cxR = 0;
        //    if (decimal.TryParse(value?.ToString() ?? "", out cxR)) return cxR;
        //    else return null;// here must be back value
        //}
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values[0] == null) return null;
            return ((decimal)values[0]).ToSF(System.Convert.ToInt32(parameter));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            decimal cxR = 0;
            if (decimal.TryParse(value?.ToString() ?? "", out cxR))
            {
                return new object[] { cxR, cxR + 1 };
            }
            else
            {
                if (targetTypes[0] == typeof(decimal?)) return null;
                else return null; // here must be back value
            }
        }





        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }


















    //public class czConvertDecimalToString : czConvertor_MarkupExtension, IValueConverter //czConvertor_MarkupExtension<czConvertDecimalToString>
    //{
    //    //public decimal CurrValue { set; private get; }

    //    //public string Calendar { set; private get; }


    //    decimal cvLast_Value;

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //        {
    //            return null;
    //        }
    //        else
    //        {
    //            cvLast_Value = ((decimal)value);
    //            return cvLast_Value.ToSF(System.Convert.ToInt32(parameter));
    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (string.IsNullOrWhiteSpace(value?.ToString() ?? ""))
    //        {
    //            if (targetType == typeof(decimal?)) return null;
    //            else return 0;
    //        }
    //        else
    //        {
    //            string cxS = (value?.ToString() ?? "").Replace(",", ".");
    //            decimal cxR = 0;
    //            if (decimal.TryParse(cxS, NumberStyles.Any, CultureInfo.InvariantCulture, out cxR)) return cxR;
    //            else return "error";// 0;// cvLast_Value;// throw new Exception("Wrong enter beach!");// return null;// here must be back value
    //        }
    //    }





    //    //public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(decimal), typeof(czConvertDecimalToString), null, BindingMode.TwoWay);

    //    //public decimal Value
    //    //{
    //    //    private get { return (decimal)GetValue(ValueProperty); }
    //    //    set { SetValue(ValueProperty, value); }
    //    //}


    //}









    //public class czConvertDecimalToString : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null) return null;
    //        return ((decimal)value).ToSF(System.Convert.ToInt32(parameter));
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;
    //        decimal cxR = 0;
    //        decimal.TryParse(value?.ToString() ?? "", out cxR);
    //        return cxR;
    //    }
    //}




    //public class czConvertDecimalToString : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null) return null;
    //        return ((decimal)value).ToSF(System.Convert.ToInt32(parameter));
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if(targetType==typeof(decimal?))
    //        {

    //        }
    //        if (value == null) return null;
    //        decimal cxR = 0;
    //        if (decimal.TryParse(value?.ToString() ?? "", out cxR)) return cxR;
    //        else return null;// here must be back value
    //    }

    //}




    //public abstract class czConvertor_MarkupExtension<T> : /*BindableObject,*/ IMarkupExtension, IValueConverter where T : class, new()
    //{
    //    //private static T _converter = null;

    //    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
    //    public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

    //    public object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        //if (_converter == null) _converter = new T();
    //        //return _converter;

    //        return this;
    //    }

    //}











    //public abstract class BindingExtension : BindingBase Binding, IValueConverter
    //{
    //    protected BindingExtension()
    //    {
    //        Source = Converter = this;
    //    }

    //    protected BindingExtension(object source) // set Source to null for using DataContext
    //    {
    //        Source = source;
    //        Converter = this;
    //    }

    //    protected BindingExtension(RelativeSource relativeSource)
    //    {
    //        RelativeSource = relativeSource;
    //        Converter = this;
    //    }

    //    public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

    //    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}





}
