using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cpADD.Ability.IAsyncModuleStartControl
{

    [czaAutoRegisterSingleInstance]
    public class czAsyncModuleStartControl : czIAsyncModuleStartControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public czAsyncModuleStartControl()
        {
            AsyncModule_Info = new ObservableCollection<czAsyncModule_Info>();
            cfTime_Out();
        }

        const int TimeOut_SP = 100;//10 sec
        int TimeOut = TimeOut_SP;
        public bool? Load_Finish { get; set; } = false;
        

        public string Status { get; set; } 
        public ObservableCollection<czAsyncModule_Info> AsyncModule_Info { get; set; }




        public async Task<bool> cfAsync_Load()
        {
            await Task.Run(() =>
            {
                do
                {
                    Thread.Sleep(300);
                } while (Load_Finish == false);
                // For visual see status
                Thread.Sleep(1000);
            });
            return Load_Finish ?? false;
        }





        public void cfSetPersents(object cxModule, int cxPersents)
        {
            string cxKey = cxModule.GetType().ToString();
            var Qk = AsyncModule_Info.Where(s => s.Module_Name == cxKey);
            if (Qk.Any()) Qk.Single().Persents = cxPersents;
            else AsyncModule_Info.Add(new czAsyncModule_Info() { Module_Name = cxKey, Persents = cxPersents });

            if (AsyncModule_Info.All(s => s.Persents >= 100))
            {
                Load_Finish = true;
                Status = "Все модули успешно загружены...";
            }
            else TimeOut = TimeOut_SP;
        }

        public void cfSetStatus(object cxModule, string cxStatus)
        {
            string cxKey = cxModule.GetType().ToString();
            var Qk = AsyncModule_Info.Where(s => s.Module_Name == cxKey);
            if (Qk.Any()) Qk.Single().Status = cxStatus;
            else AsyncModule_Info.Add(new czAsyncModule_Info() { Module_Name = cxKey, Status = cxStatus });
        }





        private void cfTime_Out()
        {
            Task.Run(() =>
            {
                do
                {
                    Thread.Sleep(100);
                    TimeOut--;
                    cfSet_Status_Wait();
                    if (TimeOut == 0) Load_Finish = null;
                } while (Load_Finish==false);

            });
        }
        private void cfSet_Status_Wait()
        {
            if (Load_Finish==false)
            {
                if (TimeOut <= 0) Status = "Модули не загружены...";
                //else Status = $"Ожидание запуска всех модулей...{Math.Round(Convert.ToDouble(TimeOut/10),2)} сек";
                else Status = $"Ожидание запуска всех модулей...{TimeOut/10} сек";
            }
        }


    }




         



}
