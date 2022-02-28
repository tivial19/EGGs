using cpADD.Ability;
using cpADD.APP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace cpADD
{
    public class czCMD_Base : czICMD_Base
    {
        Stopwatch cvStopWatch;

        protected string CMD_NAME;
        protected Func<object, Task> RUN_Action;


        protected czITrace _Trace;
        protected czIAPP_MSG _APP_MSG;
        protected czIAPP_Thread _APP_Thread;


        public czCMD_Base(Func<object, Task> cxRUN_Action, [CallerMemberName] string cxName = "NoNameCMD") 
        {
            RUN_Action = cxRUN_Action;
            CMD_NAME = cxName;
        }

        public czCMD_Base(Func<Task> cxRUN_Action, [CallerMemberName] string cxName = "NoNameCMD") : this(async (d) => await cxRUN_Action(), cxName)
        {

        }

        public czCMD_Base(Action<object> cxRUN_Action, [CallerMemberName] string cxName = "NoNameCMD") : this(async (d) => { await Task.Yield(); cxRUN_Action(d); }, cxName)
        {

        }

        public czCMD_Base(Action cxRUN_Action, [CallerMemberName] string cxName = "NoNameCMD") : this(async (d) => { await Task.Yield(); cxRUN_Action(); }, cxName)
        {

        }



        public void cfInject(czITrace cxTrace, czIAPP_Thread cxAPP_Thread, czIAPP_MSG cxAPP_MSG)
        {
            _APP_MSG = cxAPP_MSG;
            _APP_Thread = cxAPP_Thread;
            _Trace = cxTrace;
        }




        public bool IsInjection_OK => _APP_MSG != null && _Trace!=null;

        public object CommandParameter { get; private set; }


        protected bool isRunning { get => _isRunning; set { _isRunning = value; cfCanExecuteChanged(); } } private bool _isRunning;


        protected virtual bool isEnable
        {
            get
            {
                return !isRunning;
            }
        }


  


        public async Task cfRUN(object cxParam = null)
        {
            CommandParameter = cxParam;

            if (isEnable)
            {
                if (!isRunning)
                {
                    try
                    {
                        cvStopWatch = Stopwatch.StartNew();
                        cfMSG($"Run({cxParam})");
                        isRunning = true;

                        await RUN_Action?.Invoke(cxParam);
                    }
                    catch (Exception cxE)
                    {
                        cfMSG("Exception: " + cxE.Message);
                        await _APP_MSG.cfMSG_OK(cxE.Message, CMD_NAME + " .Ошибка при выполнении команды.");
                    }
                    finally
                    {
                        isRunning = false;
                        cvStopWatch?.Stop();
                        cfMSG($"End({cxParam}) {cvStopWatch.Elapsed.ToString("mm\\:ss\\.fff")}");
                    }
                }
            }
        }



        private void cfMSG(string cxMSG)
        {
            _Trace?.cfTrace($"{CMD_NAME}: {cxMSG}");
        }









        protected virtual void cfCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }




//ICommand


        public  event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            return isEnable;
        }



        public  void Execute(object cxParam)
        {
            _ = cfRUN(cxParam);
        }




    }


}
