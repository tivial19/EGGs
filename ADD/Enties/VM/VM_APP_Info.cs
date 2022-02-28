using cpADD.Ability;
using cpADD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpADD
{
    public class czVM_APP_Info: czVM, czIAPP_Info, czITrace//, czIAPP_Logger
    {
        //public static czVM_App_Base THIS { get; private set; }


        czIAPP _APP;
        czIAsyncModuleStartControl _AMS;
        //czILogger _Logger;

        public czVM_APP_Info(/*czILogger cxLogger,*/ czIAsyncModuleStartControl cxAMS, czIAPP cxAPP, czITrace cxTrace):base(cxAPP,cxAPP, cxTrace)
        {
            _APP = cxAPP;
            _AMS = cxAMS;
            //_Logger = cxLogger;

            //THIS = this;
        }


        //czIIOC_Container czIAPP_base.IOC_Container => _APP.IOC_Container;



        //public event Action ceAppShutDown {add{_APP.ceAppShutDown += value;}remove{_APP.ceAppShutDown -= value;}}

        //public bool isShutDowning => _APP.isShutDowning;



        public TimeSpan Time_from_Start => _APP.Time_from_Start;

        public string Path_App => _APP.Path_App;

        public string Path_Cache => _APP.Path_Cache;

        public string Path_Download => _APP.Path_Download;

        public string Name => _APP.Name;

        public string Package_Name => _APP.Package_Name;

        public string Version => _APP.Version;

        public string Device_Name => _APP.Device_Name;

        public string Title => _APP.Title;

        public string Year => _APP.Year;

        public string[] Shared_PRJ_Info => _APP.Shared_PRJ_Info;





        public ObservableCollection<string> MSGs => _Trace.MSGs;

        //public ObservableCollection<string> Events => _APP.Events;




        public string Status => _AMS.Status;

        public ObservableCollection<czAsyncModule_Info> AsyncModule_Info => _AMS.AsyncModule_Info;






        public czICMD_Base ccSAVE_Modules_to_File { get => _ccSAVE_Modules_to_File ?? (_ccSAVE_Modules_to_File = new czCMD_Base(_APP.cfSAVE_ALL)); }private czICMD_Base _ccSAVE_Modules_to_File;





        public czICMD_Base ccExit { get => _ccExit ?? (_ccExit = new czCMD_Base(cfExit)); } private czICMD_Base _ccExit;
        public void cfExit()
        {
            _APP.cfExit();
        }










        public czICMD_Base ccUpdate { get => _ccUpdate ?? (_ccUpdate = new czCMD_Base(cfUpdate)); } private czICMD_Base _ccUpdate;
        public async Task cfUpdate()
        {
            await _APP.cfUpdate();
        }

        public czICMD_Base ccUpload { get => _ccUpload ?? (_ccUpload = new czCMD_Base(cfUpload)); }private czICMD_Base _ccUpload;
        public async Task cfUpload()
        {
            await _APP.cfUpload();
        }













        public void cfLog(params string[] cxEvents)
        {
            throw new NotImplementedException();
        }

        public void cfTrace(string cxMSG)
        {
            throw new NotImplementedException();
        }

        public void cfTrace_Time(string cxMSG)
        {
            throw new NotImplementedException();
        }
    }

}
