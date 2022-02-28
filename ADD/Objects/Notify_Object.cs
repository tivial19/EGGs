using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cpADD
{




    public class czNotify_Object : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public czNotify_Object()
        {
            PropertyChanged += (s, e) => cfPropertyChanged(e.PropertyName);
        }


        public virtual void cfPropertyChange([CallerMemberName] string cxName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(cxName));
        }


        protected virtual void cfPropertyChanged(string cxName)
        {

        }


        //protected bool isNotDesignMode { get => !(bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue; }
    }




}
