//using Android.Runtime;
using cpADD;
using cpADD.Ability;
using cpADD.APP;
using cpADD.EXT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace cpXamarin_APP
{

    //ADD to main app to STARTUP

    //public App(czIActivity cxActivity)
    //{
    //    InitializeComponent();

    //    czAPP cxAPP = new czAPP(cxActivity, this, typeof(czView_Main));
    //    var cxReg = new czIOC_Autofac_Register(cxAPP);
    //    cxReg.cfRegister();
    //    _=cxReg.cfBuild_and_Inject(cxAPP.cfInject);
    //}


    public partial class czAPP : czAPP_base, czIAPP
    {
        public static Application _app { get; private set; }

        czIActivity _activity;
        

        //For Test
        public czAPP(string cxPath) : base(cxPath) { }


        public czAPP(czIActivity cxActivity, Application cxApp, Type cxMainView, string cxName = null) : base(cfGet_APP_Info_Struct(cxActivity, cxName),cxMainView)
        {
            _activity = cxActivity;
            _app = cxApp;

            _activity.cfSet_UnhandledExceptionRaiser(cfUnhandledException, () => (_app != null && _app.MainPage != null));
        }


        public czAPP(czIActivity cxActivity, Application cxApp, string cxName=null) :this(cxActivity,cxApp,null,cxName) { }





        private static czAPP_Info_Create_Struct cfGet_APP_Info_Struct(czIActivity cxActivity, string cxName)
        {
            czAPP_Info_Create_Struct cxInfo= new czAPP_Info_Create_Struct()
            { 
                Path_App=cxActivity.Path_APP,
                Path_Cache=cxActivity.Path_Cache,
                Path_Download=cxActivity.Path_Download,
                Name=string.IsNullOrWhiteSpace(cxName) ? AppInfo.Name : cxName,
                Version=AppInfo.VersionString,
                Package_Name=AppInfo.PackageName,
                Device_Name=DeviceInfo.Model//=DeviceInfo.Name;
            };
            return cxInfo;
        }


        protected async override Task cfShow_Exception(string cxText, string cxTitle)
        {
            await cfMSG_OK(cxText, cxTitle);
        }






        public override async Task<bool> cfInject(czIIOC_Container cxIOC_container)
        {
            if (true == await base.cfInject(cxIOC_container))
            {
                if(_MainViewType!=null) _app.MainPage = (Page)cxIOC_container.cfGetInstance(_MainViewType);

                //await cfMSG_Select(new string[] { "1", "2", "3" });
                //await cfMSG_OK("test");
                //await cfTEST_MSG();

                cfMSG_Info($"Приложение {Title} загружено.", true);
                return true;
            }
            else return false;
        }




        public async override Task<bool> cfis_ShutDown()
        {
            bool cxR = await _app.MainPage.DisplayAlert("Завершение работы", "Вы хотите завершить работу?", "Выход", "Отмена");

            //string cxRS = await MainPage.DisplayActionSheet("Завершение работы","cancel", "Вы хотите завершить работу?", "Выход", "Отмена","1","2");
            ////string cxRS= await MainPage.DisplayActionSheet("title", "cancel", "destruction", new string[] { "button1", "button2" });
            //bool cxR = false;
            
            return cxR;
        }

        public override void cfShutDown()
        {
            _activity.cfShutDown();
        }





        public void cfRun_File(string cxFile, string cxArg=null)
        {
            //var x = await Launcher.TryOpenAsync(new Uri(cxFile));
            //var y = await Launcher.TryOpenAsync(cxFile);

            _ = Launcher.OpenAsync(new OpenFileRequest() { File = new ReadOnlyFile(cxFile) });


            //if (Path.GetExtension(cxFile).ToLower() == ".apk") cfRun_APK(cxFile);
            //else Launcher.OpenAsync(new OpenFileRequest() { File = new ReadOnlyFile(cxFile) }); //throw new NotImplementedException("Unknown file Extension!");
        }

        public async void cfShare_File(string cxFile, string cxTitle = "Файл")
        {
            string cxF = cxFile;
            if (string.IsNullOrWhiteSpace(cxF) || !System.IO.File.Exists(cxF))
            {
                cxF = await cfMSG_Select_File();
                if (string.IsNullOrWhiteSpace(cxF)) return;
            }

            if (System.IO.File.Exists(cxF)) await Share.RequestAsync(new ShareFileRequest(cxTitle, new ShareFile(cxF)));
            else await cfMSG_OK($"Файл не найден {cxF}");
        }

        public void cfSet_Clipboard(string cxText)
        {
            Clipboard.SetTextAsync(cxText);
        }





        public override void cfMainThread(Action cxAction)
        {
            if (_app?.Dispatcher?.IsInvokeRequired ?? false) Device.BeginInvokeOnMainThread(cxAction); else cxAction();
        }

        public override Task cfMainThread(Func<Task> cxFunc)
        {
            if (_app?.Dispatcher?.IsInvokeRequired ?? false) return Device.InvokeOnMainThreadAsync(cxFunc); else return cxFunc();
        }

        public override Task<T> cfMainThread<T>(Func<Task<T>> cxFunc)
        {
            if (_app?.Dispatcher?.IsInvokeRequired ?? false) return Device.InvokeOnMainThreadAsync(cxFunc); else return cxFunc();
        }






        private bool? cfGetMSG_Result(int? cxAlertDialogR)
        {
            return cxAlertDialogR switch
            {
                1 => true,       //yes btn
                2 => false,       //no btn
                _ => null,      //missclick, cancel(0)
            };
        }




        public void cfMSG_Info(string cxText, bool cxLength_Long)
        {
            cfMainThread(()=> _activity.cfToast(cxText,cxLength_Long));
        }

        public async Task<bool?> cfMSG_OK(string cxText, string cxTitle = "Внимание")
        {
            int? cxR= await cfMainThread<int?>(() => _activity.cfAlertDialog(cxText, cxTitle, "Ok", null, null));

            return cfGetMSG_Result(cxR);

            //if (_app!=null && _app.MainPage != null)
            //{
            //    cxTitle ??= Title;
            //    await cfMainThread(()=> _app.MainPage?.DisplayAlert(cxTitle, cxText, "OK"));
            //}
        }

        public async Task<bool?> cfMSG_YesCancel(string cxText, string cxTitle = "Внимание")
        {
            int? cxR = await cfMainThread<int?>(() => _activity.cfAlertDialog(cxText, cxTitle, cxNo: null));

            return cfGetMSG_Result(cxR);
        }

        public async Task<bool?> cfMSG_YesNo(string cxText, string cxTitle = "Внимание")
        {
            int? cxR = await cfMainThread<int?>(()=> _activity.cfAlertDialog(cxText, cxTitle,cxCancel:null));

            return cfGetMSG_Result(cxR);
        }

        public async Task<bool?> cfMSG_YesNoCancel(string cxText, string cxTitle = "Внимание")
        {
            int? cxR = await cfMainThread<int?>(() => _activity.cfAlertDialog(cxText, cxTitle));

            return cfGetMSG_Result(cxR);
        }

        public async Task<bool?> cfMSG_YesCustom(string cxText, string cxCustomButton, string cxTitle = "Внимание")
        {
            int? cxR = await cfMainThread<int?>(() => _activity.cfAlertDialog(cxText, cxTitle,cxNo:null, cxCancel: cxCustomButton));
            return cxR switch
            {
                1 => true,       //yes btn
                0 => false,       //cancel(0) btn
                _ => null,      //missclick 
            };
        }

        public Task<int?> cfMSG_YesNoCustom(string cxText, string cxCustomButton, string cxTitle = "Внимание")
        {
            //1 - OK, 2- No, 0 - Custom, Null- missclick
            return cfMainThread<int?>(() => _activity.cfAlertDialog(cxText, cxTitle,cxCancel:cxCustomButton));
        }

        public Task<int?> cfMSG_Universal(string cxText, string cxButton1="Да", string cxButton2="Нет", string cxButton0="Отмена", string cxTitle = "Внимание")
        {
            //1 - OK, 2- No, 0 - Custom, Null- missclick
            return cfMainThread<int?>(() => _activity.cfAlertDialog(cxText, cxTitle,cxButton1,cxButton2,cxButton0));
        }


        public async Task<(bool isSelected, T Value)> cfMSG_Select_Object<T>(T[] cxObjects, Func<T, string> cxGet_Text, string cxTitle = "Сделай свой выбор:", string cxCancel = "Отмена", string cxClear = "")
        {
            string[] cxTexts = cxObjects.Select(s => cxGet_Text(s)).ToArray();
            string cxRS = await cfMSG_Select(cxTexts, cxTitle,cxCancel,cxClear);

            if (string.IsNullOrWhiteSpace(cxRS)) return (false, default(T));

            int cxIndex = Array.IndexOf(cxTexts, cxRS);
            if (cxIndex < 0 || cxIndex > cxTexts.Length - 1) throw new Exception("cfMSG_Select_Object Ошибка при выборе. Индекс вне диапозона!");

            return (true, cxObjects[cxIndex]);
        }

        public async Task<string> cfMSG_Select(string[] cxToSelect, string cxTitle = "Сделай свой выбор:", string cxCancel = "Отмена", string cxClear = null)
        {
            if (_app != null && _app.MainPage != null)
            {
                string cxR = await cfMainThread<string>(() => _app.MainPage.DisplayActionSheet(cxTitle, cxCancel, cxClear, cxToSelect));
                if (cxR != cxCancel) return cxR;
            }
            return null;
        }


        public async Task<string> cfMSG_Select_File(string cxFile_Filter = null, string cxDir = null)
        {
            string cxPath = cxDir;
            if (string.IsNullOrWhiteSpace(cxPath) || !Directory.Exists(cxPath)) cxPath = Path_App;

            string[] cxFiles = cxFile_Filter==null?Directory.GetFiles(cxPath): Directory.GetFiles(cxPath, cxFile_Filter);
            var FileNames = cxFiles.Select(s => Path.GetFileName(s));
            if (cxFiles.Any())
            {
                string cxFileName = await cfMSG_Select(FileNames.ToArray());
                if (!string.IsNullOrWhiteSpace(cxFileName)) return Path.Combine(Path_App, cxFileName);
            }
            return null;
        }


        public Task<string> cfMSG_Input(string cxText, string cxTitle = "Введите значение:", string cxPlaceHolder = null, string cxOK = "OK", string cxCancel = "Отмена", bool cxKeyBoardText=false, string cxInitialValue="")
        {
            // cancel=null; accept="";
            if (_app != null && _app.MainPage != null)
                return cfMainThread<string>(() => _app.MainPage.DisplayPromptAsync(cxTitle, cxText, cxOK, cxCancel, cxPlaceHolder, keyboard: cxKeyBoardText ? Keyboard.Text : Keyboard.Numeric, initialValue:cxInitialValue));
            else return null;
        }








        public async Task cfTEST_MSG()
        {
            int t = 0;
            do
            {
                var d = await cfDateDialog(DateTime.Today);
                t++;

            } while (true);
            


            bool? cxMSG_OK_OK = await cfMSG_OK("OK", "cfMSG_OK");
            bool? cxMSG_OK_Miss = await cfMSG_OK("MissClick", "cfMSG_OK");

            bool? cxMSG_YesCancel_Yes = await cfMSG_YesCancel("YES", "cfMSG_YesCancel");
            bool? cxMSG_YesCancel_Cancel = await cfMSG_YesCancel("Cancel", "cfMSG_YesCancel");
            bool? cxMSG_YesCancel_Miss = await cfMSG_YesCancel("MissClick", "cfMSG_YesCancel");

            bool? cxMSG_YesNo_Yes = await cfMSG_YesNo("YES", "cfMSG_YesNo");
            bool? cxMSG_YesNo_No = await cfMSG_YesNo("No", "cfMSG_YesNo");
            bool? cxMSG_YesNo_Miss = await cfMSG_YesNo("MissClick", "cfMSG_YesNo");

            bool? cxMSG_YesNoCancel_Yes = await cfMSG_YesNoCancel("YES", "cfMSG_YesNoCancel");
            bool? cxMSG_YesNoCancel_No = await cfMSG_YesNoCancel("No", "cfMSG_YesNoCancel");
            bool? cxMSG_YesNoCancel_Cancel = await cfMSG_YesNoCancel("Cancel", "cfMSG_YesNoCancel");
            bool? cxMSG_YesNoCancel_Miss = await cfMSG_YesNoCancel("MissClick", "cfMSG_YesNoCancel");

            List<int?> cxLC = new List<int?>();

            for (int i = 0; i < 4; i++)
            {
                int? x = await cfMSG_YesNoCustom(i.ToString(), "custom");
                cxLC.Add(x);
            }

        }







        public Task<DateTime?> cfDateDialog(DateTime cxDateStart, DateTime? cxbtn2 = null, DateTime? cxbtn3 = null, string cxTitle = null, string cxText = null)
        {
            return _activity.cfDateDialog(cxDateStart, cxbtn2, cxbtn3, cxTitle, cxText);
        }

        public Task<TimeSpan?> cfTimeDialog(TimeSpan cxTimeStart, TimeSpan? cxbtn2 = null, TimeSpan? cxbtn3 = null, string cxTitle = null, string cxText = null)
        {
            return _activity.cfTimeDialog(cxTimeStart, cxbtn2, cxbtn3, cxTitle, cxText);
        }


    }
}
