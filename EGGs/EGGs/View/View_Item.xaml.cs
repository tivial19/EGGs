using cpWORK;
using cpWORK.Enties;
using cpXamarin_APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EGGs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class czView_Item : czContentPage
    {
        //czICMD_Base _Cmd_Ok;

        //czVM_Work _VM_Work;


        public czView_Item(czItem cxItem, czVM_Work cxVM_Work)
        {
            InitializeComponent();

            BindingContext = cxItem;
            btnOK.BindingContext = cxVM_Work;
        }



    }
}