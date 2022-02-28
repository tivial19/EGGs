using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cpXamarin_APP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class czPickerCustom : czPickerCustom_Base
    {
        public czPickerCustom()
        {
            //_txtEnter = txtEnter;
            //_lblText = lblText;
            

            InitializeComponent();

            TextColor = Color.Black;
        }





        public string TextLabel
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }



        public Type Value_Type
        {
            get { return txtEnter.Value_Type; }
            set { txtEnter.Value_Type = value; }
        }



        public Keyboard Keyboard
        {
            get { return txtEnter.Keyboard; }
            set { txtEnter.Keyboard = value; }
        }


        public bool isReadOnly
        {
            get { return txtEnter.IsReadOnly; }
            set { txtEnter.IsReadOnly = value; }
        }



        public string Placeholder
        {
            get { return txtEnter.Placeholder; }
            set { txtEnter.Placeholder = value; }
        }


        public Color PlaceholderColor
        {
            get { return txtEnter.PlaceholderColor; }
            set { txtEnter.PlaceholderColor = value; }
        }

        public Color TextColor
        {
            get { return txtEnter.TextColor; }
            set { txtEnter.TextColor = value; }
        }



        public override void cfSelected(object cxValue)
        {
            //txtEnter.Text = cxValue?.ToString()??"";

            if (cxValue == null) txtEnter.Text = "";
            else txtEnter.Value = cxValue;
        }




    }
}