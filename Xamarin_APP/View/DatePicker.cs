using cpADD;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace cpXamarin_APP
{
    public class czDatePicker:Label
    {
        const string cvFormat = "dd.MM.yy";


        czIAPP_MSG _APP_MSG;

        public czDatePicker() 
        {
            FontSize = 18;
            //Margin = new Thickness(5, 5, 5, 5);
            VerticalTextAlignment = TextAlignment.Center;
            TapGestureRecognizer singleTap = new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1
            };

            this.GestureRecognizers.Add(singleTap);
            singleTap.Tapped += Label_Clicked;
        }



        public void cfInject(czIAPP_MSG cxAPP_MSG)
        {
            _APP_MSG = cxAPP_MSG;
        }




        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(DateTime), typeof(czDatePicker), default(DateTime), BindingMode.TwoWay, propertyChanged: OnValue_Changed);

        [PropertyChanged.SuppressPropertyChangedWarnings]
        static void OnValue_Changed(BindableObject cxbindable, object cxoldValue, object cxnewValue)
        {
            if (cxbindable != null && cxnewValue != null) ((Label)cxbindable).Text = ((czDatePicker)cxbindable).Value.ToString(cvFormat); 
        }

        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); /*Text = value.ToString(cvFormat);*/ }
        }






        public DateTime QuickDate
        {
            get { return (DateTime)GetValue(QuickDateProperty); }
            set { SetValue(QuickDateProperty, value); }
        }
        public static readonly BindableProperty QuickDateProperty = BindableProperty.Create("QuickDate", typeof(DateTime), typeof(czDatePicker), default(DateTime), BindingMode.TwoWay);    //ADD Referense System.Windows.Forms.dll => using System.Windows.Controls;




        //public DateTime Current
        //{
        //    get { return _Current; }
        //    set { _Current = value; Text = _Current.ToString(cvFormat); }
        //}
        //private DateTime _Current;



        private async void Label_Clicked(object sender, EventArgs e)
        {
            DateTime? d = await _APP_MSG.cfDateDialog(Value, QuickDate);
            if (d != null) Value = d.Value; 
        }


    }
}
