using cpADD.Ability;
using cpADD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cpADD
{



    public class czAPP_model : czAPP_model_base, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public czAPP_model(czIAPP cxAPP, czITrace cxTrace) : base(cxAPP,cxTrace)
        {
 
        }

        public czAPP_model()
        {
        }

        //for PropertyChange
        public override string Status
        {
            get { return base.Status; }
            set { base.Status = value; }
        }



        public virtual void cfPropertyChange([CallerMemberName] string cxName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(cxName));
        }
    }







    public class czAPP_model_base
    {
        protected czITrace _Trace;
        protected czIAPP _APP;

        public czAPP_model_base(czIAPP cxAPP, czITrace cxTrace)
        {
            _APP = cxAPP;
            _Trace = cxTrace;

            _APP.ceAppShutDown += cfDeactivate;
        }

        public czAPP_model_base()
        {
            //var x = this;
        }


        protected bool isStatusCopyTrace { get; set; } = false;


        public virtual string Status
        {
            get { return _Status; }
            set { _Status = value; if(isStatusCopyTrace) _Trace.cfTrace(this.GetType().Name + ": " + value); }
        }
        private string _Status;


        protected virtual Task cfDeactivate()
        {
            return Task.Delay(1);
        }



        Stopwatch cvSW;
        public void cfStart_Timer()
        {
            if(!cvSW?.IsRunning??true) cvSW = Stopwatch.StartNew();
        }

        public string cfStop_Timer()
        {
            cvSW?.Stop();
            return cvSW?.Elapsed.ToString("mm\\:ss\\.fff")??"Timer is NULL";
        }



    }





}
