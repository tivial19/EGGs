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

    //public class czModel_CMD_EXEPTION : Exception { public czModel_CMD_EXEPTION(string cxMSG) : base(cxMSG) { } }

    public class czCMD_Simple :  czICMD_Simple
    {

        Action<object> RUN_Action;



        public czCMD_Simple()
        {

        }

        public czCMD_Simple(Action<object> cxRUN_Action) : base()
        {
            RUN_Action = cxRUN_Action;
        }

        public czCMD_Simple(Action cxRUN_Action) : this((d)=>cxRUN_Action())
        {

        }



        




//ICommand


        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            return true;
        }


        public void Execute(object cxParam)
        {
            RUN_Action(cxParam);
        }




    }






}