using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace cpADD
{



    public interface czICMD_CanStop : czICMD_Base
    {
        void cfSTOP(object cxParam);

        ICommand ccSTOP { get; }
    }


    public interface czICMD_Base: ICommand
    {
        object CommandParameter { get; }

        Task cfRUN(object cxParam = null);
    }


    public interface czICMD_Simple : ICommand
    {

    }




}
