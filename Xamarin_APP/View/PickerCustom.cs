using cpADD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace cpXamarin_APP
{
    public partial class czPickerCustom_Base : ContentView
    {

        //czIAPP_MSG _APP_MSG;

        public czPickerCustom_Base()
        {

        }



        //public static void cfInject(IList<Xamarin.Forms.View> cxChildren, czIAPP_MSG cxAPP_MSG)
        //{
        //    //var Qp = cxChildren.Where(t => typeof(czPickerCustom_Base).IsAssignableFrom(t.GetType()));
        //    //if (!(Qp?.Any() ?? false)) throw new Exception("czPickerCustom_Base cfInject cxChildren==NULL");
        //    //foreach (var item in Qp) (item as czPickerCustom_Base).cfInject(cxAPP_MSG);
        //}


        //public void cfInject(czIAPP_MSG cxAPP_MSG)
        //{
        //    _APP_MSG = cxAPP_MSG;
        //}


        public string Title { get; set; } = "Выбери:";


        public bool isCanbeClear { get; set; } = false;




        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(object), typeof(czEntry_Completed), null, BindingMode.TwoWay);

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }



        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(System.Collections.IEnumerable), typeof(czPickerCustom_Base), null, BindingMode.TwoWay);

        public System.Collections.IEnumerable ItemsSource
        {
            get { return (System.Collections.IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }



        public void btnSelect(object sender, EventArgs e)
        {
            cfMSG_Select_Clear(ItemsSource, isCanbeClear); 
        }





        public virtual void cfSelected(object cxValue){}

        public async void cfMSG_Select_Clear(System.Collections.IEnumerable cxSource, bool isCanbeClear)
        {

            ContentPage cxCP = new ContentPage
            {
                //BackgroundColor = Color.FromHex("#D9000000"),
                Padding = new Thickness(20)
            };
           

            Xamarin.Forms.ListView cxLV = new Xamarin.Forms.ListView { ItemsSource = cxSource};
            cxLV.ItemTapped += (s, e) =>
            {
                cfSelected(cxLV.SelectedItem);                                                               //SELECTED
                cxCP.Navigation.PopModalAsync();
            };

            Xamarin.Forms.Button cxbtn = new Xamarin.Forms.Button { Text = "Очистить" };
            cxbtn.Clicked += (s, e) =>
            {
                cxCP.Navigation.PopModalAsync();
                if (isCanbeClear) cfSelected(null);
                else throw new czAPP_Exception_Normal("Очистка в данном поле запрещена!");// _APP_MSG.cfMSG_Info("Очистка в данном поле запрещена!");
            };


            StackLayout cxSL = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { cxLV, cxbtn }
            };
            cxCP.Content = cxSL;

            await czAPP._app.MainPage.Navigation.PushModalAsync(cxCP, false);
        }















        //public string TextLabel
        //{
        //    get { return _lblText.Text; }
        //    set { _lblText.Text = value; }
        //}



        //public Type Value_Type
        //{
        //    get { return _txtEnter.Value_Type; }
        //    set { _txtEnter.Value_Type = value; }
        //}



        //public Keyboard Keyboard
        //{
        //    get { return _txtEnter.Keyboard; }
        //    set { _txtEnter.Keyboard = value; }
        //}


        //public bool isReadOnly
        //{
        //    get { return _txtEnter.IsReadOnly; }
        //    set { _txtEnter.IsReadOnly = value; }
        //}



        //public string Placeholder
        //{
        //    get { return _txtEnter.Placeholder; }
        //    set { _txtEnter.Placeholder = value; }
        //}


        //public Color PlaceholderColor
        //{
        //    get { return _txtEnter.PlaceholderColor; }
        //    set { _txtEnter.PlaceholderColor = value; }
        //}

        //public Color TextColor
        //{
        //    get { return _txtEnter.TextColor; }
        //    set { _txtEnter.TextColor = value; }
        //}






    }

}
