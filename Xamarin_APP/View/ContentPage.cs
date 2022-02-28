using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace cpXamarin_APP
{
    public class czContentPage : czContentPage_Base
    {
        public czContentPage()
        {

        }


        protected override bool OnBackButtonPressed()
        {
            cfHide();
            return true;
        }



        public void cfSet_VM_Resources(object cxVM)
        {
            Resources[cxVM.GetType().Name.Substring(2)] = cxVM;
        }





        //public async void cfShow(INavigation cxNavigation)
        //{
        //    await cxNavigation.PushModalAsync(this,false);
        //}

        //public void cfShow_Context(INavigation cxNavigation, object cxBindingContext)
        //{
        //    cfSet_Context(cxBindingContext);
        //    cfShow(cxNavigation);
        //}



        //public virtual void cfSet_Context(object cxBindingContext)
        //{
        //    BindingContext = null;
        //    BindingContext = cxBindingContext;
        //}



    }



    public class czContentPage_Base : ContentPage
    {
        public czContentPage_Base()
        {
            //Views ??= new List<czContentPage_Base>();
            //Views.Add(this);
        }


        //public static List<czContentPage_Base> Views { get; set; }
        //public static T cfGet_View<T>()
        //{
        //    return Views.OfType<T>().Single();
        //}




        public virtual void cfHide()
        {
            Navigation.PopModalAsync(false);
        }

        public async void cfShow(Page cxView)
        {
            await Navigation.PushModalAsync(cxView, false);
        }

        public async void cfShow_Context(Page cxView, object cxBindingContext)
        {
            cxView.BindingContext = null;
            cxView.BindingContext = cxBindingContext;
            await Navigation.PushModalAsync(cxView, false);
        }





        public void btnHide(object sender, EventArgs e)
        {
            cfHide();
        }

    }








    public class czCarouselPage : CarouselPage
    {
        public czCarouselPage()
        {
            //Views ??= new List<czCarouselPage>();
            //Views.Add(this);
        }


        //public static List<czCarouselPage> Views { get; set; }
        //public static T cfGet_View<T>()
        //{
        //    return Views.OfType<T>().Single();
        //}




        protected override bool OnBackButtonPressed()
        {
            cfHide();
            return true;
        }




        public virtual void cfHide()
        {
            Navigation.PopModalAsync(false);
        }

        public async void cfShow(Page cxView)
        {
            await Navigation.PushModalAsync(cxView, false);
        }

        public async void cfShow(INavigation cxNavigation)
        {
            await cxNavigation.PushModalAsync(this, false);
        }




        public void btnHide(object sender, EventArgs e)
        {
            cfHide();
        }
    }




}