using cpADD;
using cpADD.APP;
using cpADD.EXT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cpXamarin_APP
{


    //public class czEntry_Complete : Entry //: czEntry_Unfocused
    //{
    //    public czEntry_Complete()
    //    {
    //        Completed += (s, e) => 
    //        { 
    //            Value = Text; 
    //        };
    //        Unfocused += (s, e) => 
    //        { 
    //            /*this.ApplyBindings();*/ 
    //            Text = Value; 
    //        };


    //        //ReturnCommand = ccReturn;
    //    }

    //    //private void txtUnfocused(object sender, FocusEventArgs e)
    //    //{
    //    //        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
    //    //        {
    //    //            if (isUnfocused)
    //    //            {
    //    //                Text = Value?.ToString();
    //    //                isUnfocused = false;
    //    //            }
    //    //            return false;
    //    //        });
           
    //    //}










    //    public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(string), typeof(czEntry_Complete), null, BindingMode.TwoWay, propertyChanged: OnValue_Changed);

    //    public string Value
    //    {
    //        get { return (string)GetValue(ValueProperty); }
    //        set { SetValue(ValueProperty, value); }
    //    }


    //    //[PropertyChanged.SuppressPropertyChangedWarnings]
    //    static void OnValue_Changed(BindableObject cxbindable, object cxoldValue, object cxnewValue)
    //    {
    //        ((czEntry_Complete)cxbindable).Text = cxnewValue?.ToString();
    //    }




    //    //public czICMD_Simple ccReturn { get => _ccReturn ?? (_ccReturn = new czCMD_Simple(cfReturn)); } private czICMD_Simple _ccReturn;
    //    //public void cfReturn(object cxParam)
    //    //{

    //    //}


    //}



    public class czEntry_Unfocused : Entry
    {
        public czEntry_Unfocused()
        {
            Unfocused +=(s,e) => this.ApplyBindings();
            //TextChanged += (s, e) => cfTextChange(e.NewTextValue);
            //OnTextChanged
        }

        protected override void OnTextChanged(string oldValue, string newValue)
        {
            try
            {
                base.OnTextChanged(oldValue, newValue);
            }
            catch (czAPP_Exception_Convertor cxE)
            {
                this.ApplyBindings();
                //throw;
            }

            
        }

    }















    //public class czEntry_Completed : Entry
    //{
    //    string cvNewTextValue;

    //    public czEntry_Completed()
    //    {
    //        TextChanged += (s, e) => cvNewTextValue = e.NewTextValue;
    //        Completed += (s, e) => { Text=cvNewTextValue; };

    //        //Completed += (s, e) => { isUnfocused = false; cfTextChange(Text); };
    //        //Unfocused += txtUnfocused;
    //    }

    //    //protected void cfTextChange(string cxNewTextValue)
    //    //{
    //    //    cvNewTextValue = cxNewTextValue;
    //    //}



    //    //bool isUnfocused = false;
    //    //private void txtUnfocused(object sender, FocusEventArgs e)
    //    //{
    //    //    if (Value?.ToString() != Text)
    //    //    {
    //    //        isUnfocused = true;
    //    //        Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
    //    //        {
    //    //            if (isUnfocused)
    //    //            {
    //    //                Text = Value?.ToString();
    //    //                isUnfocused = false;
    //    //            }
    //    //            return false;
    //    //        });
    //    //    }
    //    //}


    //}














    public class czEntry_Completed : czEntry_base
    {
        public czEntry_Completed()
        {
            Completed += (s, e) => { isUnfocused = false; cfTextChange(Text); };
            Unfocused += txtUnfocused;
        }


        bool isUnfocused = false;
        private void txtUnfocused(object sender, FocusEventArgs e)
        {
            if (Value?.ToString() != Text)
            {
                isUnfocused = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                {
                    if (isUnfocused)
                    {
                        Text = Value?.ToString();
                        isUnfocused = false;
                    }
                    return false;
                });
            }
        }

    }






    public class czEntry : czEntry_base
    {
        public czEntry()
        {
            TextChanged += (s, e) => cfTextChange(e.NewTextValue);
        }

    }






    public class czEntry_base : Entry
    {
        Type default_Numeric_Type = typeof(decimal);
        public czEntry_base()
        {

        }


        public Type Value_Type { get; set; } = typeof(string);
        public bool isNullable { get; set; } = false;
        public bool isCanbeSpace { get; set; } = false;



        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Keyboard) && Keyboard == Keyboard.Numeric) Value_Type = default_Numeric_Type;//default Keyboard.Numeric
        }




        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(object), typeof(czEntry_Completed), null, BindingMode.TwoWay, propertyChanged: OnValue_Changed);

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        [PropertyChanged.SuppressPropertyChangedWarnings]
        static void OnValue_Changed(BindableObject cxbindable, object cxoldValue, object cxnewValue)
        {
            if (cxbindable != null)
            {
                if (cxnewValue != null)
                {
                    if (((czEntry_base)cxbindable).Text != cxnewValue.ToString())
                        ((czEntry_base)cxbindable).Text = cxnewValue?.ToString();
                }
                else
                {
                    if ((((czEntry_base)cxbindable).isNullable || ((czEntry_base)cxbindable).Value_Type == typeof(string)) && ((czEntry_base)cxbindable).Text != "") ((czEntry_base)cxbindable).Text = "";
                }
            }
        }






        int I = 0;
        double D = 0;
        decimal Dec = 0;

        const string cxDefaultNumberText = "0";


        protected void cfTextChange(string cxNewTextValue)
        {
            if (string.IsNullOrEmpty(cxNewTextValue))
            {
                if (isNullable) { Value = null; return; }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(cxNewTextValue))
                {
                    if (!isCanbeSpace || Value_Type != typeof(string))
                    {
                        if (isNullable) { Value = null; return; }
                        else cxNewTextValue = "";
                    }
                }
            }

            cfSetValueByType(cxNewTextValue);



            void cfSetValueByType(string cxText)
            {
                if (cxText == null) cxText = "";

                if (Value_Type == typeof(string)) Value = cxText;
                else
                {
                    if (string.IsNullOrWhiteSpace(cxText)) { cxText = cxDefaultNumberText; Text = cxText; }

                    if (Value_Type == typeof(int))
                    {
                        if (int.TryParse(cxText, out I)) Value = I;
                        else Text = Value?.ToString();// if Value==null then Text=null
                    }

                    if (Value_Type == typeof(double))
                    {
                        if (double.TryParse(cxText, out D)) Value = D;
                        else Text = Value?.ToString();
                    }

                    if (Value_Type == typeof(decimal))
                    {
                        if (decimal.TryParse(cxText, out Dec)) Value = Dec;
                        else Text = Value?.ToString();
                    }
                }
            }



        }







    }



}
