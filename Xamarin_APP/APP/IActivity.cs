using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpXamarin_APP
{
    public interface czIActivity
    {
        string Path_APP { get; }
        string Path_Cache { get; }
        string Path_Download { get; }


        void cfSet_UnhandledExceptionRaiser(Action<Exception, string> cxFunction, Func<bool> cxHandled);
        
        
        void cfToast(string cxText, bool cxLength_Long);


        Task<int?> cfAlertDialog(string cxText, string cxTitle, string cxYes = "Да", string cxNo = "Нет", string cxCancel = "Отмена");
        Task<DateTime?> cfDateDialog(DateTime cxDateStart, DateTime? cxbtn2 = null, DateTime? cxbtn3 = null, string cxTitle = null, string cxText = null);
        Task<TimeSpan?> cfTimeDialog(TimeSpan cxTimeStart, TimeSpan? cxbtn2 = null, TimeSpan? cxbtn3 = null, string cxTitle = null, string cxText = null);
        void cfShutDown();
    }


}