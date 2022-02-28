using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace cpADD.Ability
{
    public interface czITrace
    {
        ObservableCollection<string> MSGs { get; }

        void cfTrace(string cxMSG);
        void cfTrace_Time(string cxMSG);
    }

}
