using cpADD;
using cpWORK;
using cpWORK.Enties;
using cpXamarin_APP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EGGs
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class czView_Main : czCarouselPage
    {
        czVM_APP_Info _VM_App;
        czVM_Work _VM_Work;
        readonly czIAPP_base _App_base;
        public czView_Main(czVM_Work cxVM_Work, czVM_APP_Info cxVM_App, czIAPP_base app_base)
        {
            InitializeComponent();

            _VM_App = cxVM_App;
            _VM_Work = cxVM_Work;
            _VM_Work._View_Item_Show = cfShow_View_Item;
            _App_base=app_base;

            BindingContext = cxVM_Work;

            btnExit.BindingContext = cxVM_App;
            btUpdate.BindingContext = cxVM_App;
        }

        protected override bool OnBackButtonPressed()
        {
            _App_base.cfExit();
            return true;
        }

        private async void cfShow_View_Item(czItem cxTable)
        {
            Page cxView = new czView_Item(cxTable, _VM_Work);
            await Navigation.PushModalAsync(cxView, false);
        }


    }
}
