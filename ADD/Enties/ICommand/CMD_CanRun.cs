using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cpADD
{
    public class czCMD_CanRun : czCMD_CanStop
    {

        Func<bool> isCanRun_mm;


        public czCMD_CanRun(Func<object, Task> cxRUN_Action, Func<bool> cxisCanRun = null, Action cxSTOP_Action = null, [CallerMemberName] string cxName = "NoNameCMD") : base(cxRUN_Action, cxSTOP_Action, cxName)
        {
            cfSet_isCanRun(cxisCanRun);
        }

        public czCMD_CanRun(Func<Task> cxRUN_Action, Func<bool> cxisCanRun = null, Action cxSTOP_Action = null, [CallerMemberName] string cxName = "NoNameCMD") : base(cxRUN_Action, cxSTOP_Action, cxName)
        {
            cfSet_isCanRun(cxisCanRun);
        }

        public czCMD_CanRun(Action<object> cxRUN_Action, Func<bool> cxisCanRun = null, Action cxSTOP_Action = null, [CallerMemberName] string cxName = "NoNameCMD") : base(cxRUN_Action, cxSTOP_Action, cxName)
        {
            cfSet_isCanRun(cxisCanRun);
        }

        public czCMD_CanRun(Action cxRUN_Action, Func<bool> cxisCanRun = null, Action cxSTOP_Action = null, [CallerMemberName] string cxName = "NoNameCMD") : base(cxRUN_Action, cxSTOP_Action, cxName)
        {
            cfSet_isCanRun(cxisCanRun);
        }





        protected void cfSet_isCanRun(Func<bool> cxisCanRun)
        {
            isCanRun_mm = cxisCanRun ?? (() => true);
            if (cxisCanRun != null) cfRun_Task_Control();
        }




        protected override bool isEnable
        {
            get
            {
                return base.isEnable && isCanRun_mm();
            }
        }









        static int cvCount_of_Tasks = 0;
        //bool cvTask_Control_is_Started = false;


        private void cfRun_Task_Control()
        {
            cvCount_of_Tasks++;
            //cvTask_Control_is_Started = true;
            Task.Run(async () =>
            {
                bool cxR1;
                bool cxR2 = !isEnable; cfCanExecuteChanged(); //for first time set false and update
                while (true)
                {
                    try
                    {
                        cxR1 = isCanRun_mm();
                        if (cxR1 != cxR2) cfCanExecuteChanged();
                        cxR2 = cxR1;
                        await Task.Delay(100);
                    }
                    catch (Exception cxE)
                    {
                        await _APP_MSG.cfMSG_OK(cxE.Message, "Ошибка при выполнении cfRun_Task_Control"); 
                    }
                }
            });
        }





        protected override void cfCanExecuteChanged()
        {
            _APP_Thread.cfMainThread(base.cfCanExecuteChanged);
        }


    }



}
