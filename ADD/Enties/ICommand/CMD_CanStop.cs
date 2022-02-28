using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace cpADD
{
    public class czCMD_CanStop : czCMD_Base, czICMD_CanStop
    {

        Action STOP_Action;


        public czCMD_CanStop(Func<object, Task> cxRUN_Action, Action cxSTOP_Action = null, [CallerMemberName] string cxName = "NoNameCMD") : base(cxRUN_Action, cxName)
        {
            cfSet_Stop_Action(cxSTOP_Action);
        }

        public czCMD_CanStop(Func<Task> cxRUN_Action, Action cxSTOP_Action = null, [CallerMemberName] string cxName = "NoNameCMD") : base(cxRUN_Action, cxName)
        {
            cfSet_Stop_Action(cxSTOP_Action);
        }

        public czCMD_CanStop(Action<object> cxRUN_Action, Action cxSTOP_Action = null, [CallerMemberName] string cxName = "NoNameCMD") : base(cxRUN_Action, cxName)
        {
            cfSet_Stop_Action(cxSTOP_Action);
        }

        public czCMD_CanStop(Action cxRUN_Action, Action cxSTOP_Action = null, [CallerMemberName] string cxName = "NoNameCMD") : base(cxRUN_Action, cxName)
        {
            cfSet_Stop_Action(cxSTOP_Action);
        }




        protected void cfSet_Stop_Action(Action cxSTOP_Action)
        {
            STOP_Action = cxSTOP_Action;
            if (STOP_Action != null) { ccSTOP = new czCMD_CanStop(cfSTOP); }
        }






        protected override bool isEnable
        {
            get
            {
                if (ccSTOP != null) (ccSTOP as czCMD_CanStop).isEnable_Manual = isRunning;
                return !isRunning && isEnable_Manual;
            }
        }




        protected bool isEnable_Manual { get => _isEnable_Manual; set { _isEnable_Manual = value; cfCanExecuteChanged(); } } private bool _isEnable_Manual = true;//Manual switch OFF CMD


        public ICommand ccSTOP { get; set; }



        public void cfSTOP(object cxParam = null)
        {
            if (isRunning) STOP_Action?.Invoke();
        }


    }

}
