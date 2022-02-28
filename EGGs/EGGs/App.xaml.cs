using cpIOC.Autofac;
using cpXamarin_APP;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EGGs
{
    public partial class App : Application
    {
        public App(czIActivity cxMainActivity)
        {
            InitializeComponent();

            czAPP _APP = new czAPP(cxMainActivity, this, typeof(czView_Main));
            czIOC_Autofac_Register cxReg = new czIOC_Autofac_Register();

            cxReg.cfRegister_ALL(_APP);
            _ = cxReg.cfBuild_and_Inject(_APP.cfInject);
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
