using cpADD.Ability;
using cpADD.EXT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.APP
{

    public class czAPP_base: czAPP_Info, czIAPP_base
    {
        protected readonly Type _MainViewType;

        protected czITrace _Trace;
        protected czISaverOPT _Save_OPT;
        protected czIRemoteFileServer _RFS;

        public event Func<Task> ceAppShutDown;
        public bool isShutDowning { get; set; } = false;



        public czAPP_base(czAPP_Info_Create_Struct cxCreate_Info, Type cxMainView) : base(cxCreate_Info)
        {
            _MainViewType=cxMainView;
            AppDomain.CurrentDomain.UnhandledException += (s, e) => { cfUnhandledException(e.ExceptionObject as Exception, "APP_CurrentDomainUnhandledException"); };
            TaskScheduler.UnobservedTaskException += (s, e) => { cfUnhandledException(e.Exception, "UnobservedTaskException"); };
        }

        public czAPP_base(czAPP_Info_Create_Struct cxCreate_Info) : this(cxCreate_Info, null) { }

        public czAPP_base(string cxPath) : this(new czAPP_Info_Create_Struct() { Path_App=cxPath}) {}
        public czAPP_base(string cxPath, string cxName, string cxVersion) : this(new czAPP_Info_Create_Struct() { Path_App=cxPath, Name = cxName, Version = cxVersion}){}
        public czAPP_base(string cxPath_App, string cxName, string cxVersion, string cxPackage_Name, string cxDevice_Name) : this(new czAPP_Info_Create_Struct() { Path_App=cxPath_App, Name = cxName, Version = cxVersion, Package_Name = cxPackage_Name, Device_Name = cxDevice_Name }){}





        public virtual async Task<bool> cfInject(czIIOC_Container cxIOC_container)
        {
            //await Task.Delay(1); //test for correct working
            bool cxR = false;
            try
            {
                //throw new Exception("test Exception start");
                cfInject(cxIOC_container.cfGetInstance_Optional<czIRemoteFileServer>(), cxIOC_container.cfGetInstance_Optional<czITrace>(), cxIOC_container.cfGetInstance_Optional<czISaverOPT>());
                if (_MainViewType != null)
                {
                    _Trace?.cfTrace_Time("Views Creating");
                    cxIOC_container.cfGetInstance(_MainViewType);
                    _Trace?.cfTrace_Time("Views Created");
                }
                _Trace?.cfTrace_Time(Assembly_Class_Name + " Created " + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + " " + System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencySymbol);
                cxR=true;
            }
            catch (Exception cxE)
            {
                await cfShow_Exception(cxE.cfGet_Message_Exception(), "Ошибка при создании приложения!");
                cfShutDown();
            }
            return cxR;
        }

        protected void cfInject(czIRemoteFileServer cxRemoteFileServer, czITrace cxTrace, czISaverOPT cxSave_OPT)
        {
            _RFS = cxRemoteFileServer;
            _Trace = cxTrace;
            _Save_OPT = cxSave_OPT;

            _Trace?.cfTrace_Time($"{Assembly_Class_Name} cfInject base");
        }







        protected virtual void cfUnhandledException(Exception cxE, string cxTitle)
        {
            string cxMsg = null;
            if (cxE is czAPP_Exception_Convertor) return;

            if (cxE is czAPP_Exception_Normal)
            {
                cxMsg = cxE.Message;
                cxTitle=((czAPP_Exception_Normal)cxE).Title;
            }
            else cxMsg = cxE.cfGet_Message_Exception() + "\r\n" + cxE.StackTrace;

            cfShow_Exception(cxMsg, cxTitle);
        }


        protected virtual Task cfShow_Exception(string cxText, string cxTitle)
        {
            Debug.WriteLine($"{cxTitle}: {cxText}");
            return Task.Delay(1); 
        }








        public virtual Task cfSave_APP_params() { return Task.Delay(1); }
        public virtual void cfShutDown() { }

        public async virtual Task<bool> cfis_ShutDown()
        {
            await Task.Delay(0);
            return true;
        }




        public virtual void cfMainThread(Action cxAction)
        {
            cxAction();
        }

        public virtual Task cfMainThread(Func<Task> cxFunc)
        {
            return cxFunc();
        }

        public virtual Task<T> cfMainThread<T>(Func<Task<T>> cxFunc)
        {
            return cxFunc();
        }









        public async void cfExit()//not task becos nothing to waiting for
        {
            try
            {
                if (isShutDowning==false && await cfis_ShutDown())
                {
                    isShutDowning = true;
                    await cfSAVE_ALL();
                    cfShutDown();
                }
            }
            catch (Exception)
            {
                isShutDowning = false;
                throw;
            }
        }

        public async Task cfSAVE_ALL()
        {
            var Qdt = ceAppShutDown?.GetInvocationList().Cast<Func<Task>>().ToArray();
            if (Qdt?.Any() ?? false) await Task.WhenAll(Qdt.Select(f=>f.Invoke()));
            if (_Save_OPT != null)
            {
                await cfSave_APP_params();
                await _Save_OPT.cfSave_File();
            }

        }

    }


}
