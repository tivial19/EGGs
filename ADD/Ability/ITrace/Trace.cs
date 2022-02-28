using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace cpADD.Ability.ITrace
{

    [czaAutoRegisterSingleInstance]
    public class czTrace: czITrace
    {
        czIAPP_Info _APP_Info;
        czIAPP_Thread _APP_Thread;

        public czTrace(czIAPP_Thread cxAPP_Thread, czIAPP_Info cxAPP_Info)
        {
            _APP_Info = cxAPP_Info;
            _APP_Thread = cxAPP_Thread;
            MSGs = new ObservableCollection<string>();
        }

        public ObservableCollection<string> MSGs { get; }



        static object _lock_Trace = new object();

        public void cfTrace(string cxMSG)
        {
            lock (_lock_Trace)
            {
                _APP_Thread.cfMainThread(() => MSGs.Add(cxMSG));
            }
        }

        public void cfTrace_Time(string cxMSG)
        {
            cfTrace(_APP_Info.Time_from_Start.ToString(@"ss") + " : " + cxMSG);
            //cfTrace(cvStopW.Elapsed.ToString(@"mm\:ss") + " - " + cxMSG);
        }


    }
}
