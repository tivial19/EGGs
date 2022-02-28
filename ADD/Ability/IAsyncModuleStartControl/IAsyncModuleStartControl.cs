using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.Ability
{
    public interface czIAsyncModuleStartControl
    {
        ObservableCollection<czAsyncModule_Info> AsyncModule_Info { get; }

        string Status { get; }
        bool? Load_Finish { get;  }

        void cfSetStatus(object cxModule, string cxStatus);
        void cfSetPersents(object cxModule, int cxPersents);
        Task<bool> cfAsync_Load();
    }








    public class czAsyncModule_Info : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public czAsyncModule_Info()
        {

        }

        public string Module_Name { get; set; }
        public string Status { get; set; }
        public int Persents { get; set; }

    }



}
