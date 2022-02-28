using cpADD.Ability;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cpADD
{


    public class czVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected czITrace _Trace;
        protected czIAPP_MSG _APP_MSG;
        protected czIAPP_Thread _APP_Thread;

        public czVM(czIAPP_Thread cxAPP_Thread, czIAPP_MSG cxAPP_MSG, czITrace cxTrace)
        {
            _APP_MSG = cxAPP_MSG;
            _APP_Thread = cxAPP_Thread;
            _Trace = cxTrace;
        }


        //protected bool isNotDesignMode { get => !(bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue; }

        public void cfIOC_Activated()
        {
            cfActivate_modules();
            cfInject_CMD();
            _Trace.cfTrace_Time($"{GetType().Name} Activated. CMD inject = {cfIsInjection_CMD_OK()}");
        }


        public virtual void cfPropertyChange([CallerMemberName] string cxName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(cxName));
        }






        private void cfInject_CMD()
        {
            var Qcc = this.GetType().GetProperties().Where(s => typeof(czICMD_Base).IsAssignableFrom(s.PropertyType));
            foreach (var cxCmd in Qcc) (cxCmd.GetValue(this) as czCMD_Base).cfInject(_Trace,_APP_Thread,_APP_MSG);
        }
        
        public bool cfIsInjection_CMD_OK()
        {
            var Qcc = this.GetType().GetProperties().Where(s => typeof(czICMD_Base).IsAssignableFrom(s.PropertyType));

            if (Qcc.Any()) return Qcc.All(p => (p.GetValue(this) as czCMD_Base).IsInjection_OK);
            else return false;
        }

        private void cfActivate_modules()
        {

            var cxFields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).
                                     Where(s => s.FieldType.IsInterface);
                                      //Where(s => _APP.IOC_Container.cfIsRegistered(s.FieldType));

            var cxINotifys = cxFields.Select(f => f.GetValue(this)).Where(s => s is INotifyPropertyChanged).Cast<INotifyPropertyChanged>();
            foreach (var cxINotify in cxINotifys) cxINotify.PropertyChanged += (s, e) => cfPropertyChange(e.PropertyName);// PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));

            //if(!cxINotifys.Any()) _APP.cfMSG_OK($"VM {GetType().Name} не содержит INotifyPropertyChanged");
        }




        //protected void cfActivate_INotify(params object[] cxObjects)
        //{
        //    var Qi = cxObjects.Where(s => s is INotifyPropertyChanged);
        //    foreach (INotifyPropertyChanged cxItem in Qi) cxItem.PropertyChanged += (s, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        //}






        public czICMD_Base ccSTOP_ALL { get => _ccSTOP_ALL ?? (_ccSTOP_ALL = new czCMD_Base(cfSTOP_ALL)); } private czICMD_Base _ccSTOP_ALL;

        public void cfSTOP_ALL()
        {
            //await Task.Yield();
            var Qcc = this.GetType().GetProperties().Where(s => typeof(czICMD_CanStop).IsAssignableFrom(s.PropertyType));
            foreach (var cxCmd in Qcc)
            {
                czICMD_CanStop cxCC = cxCmd.GetValue(this) as czICMD_CanStop;
                cxCC.cfSTOP("STOP ALL from VM");
            }
        }


    }







}
