using cpXamarin_APP;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
//using Android.Provider;

//using AndroidX.Core.Content;
//using Android.Support.V4.Content;
//using Xamarin.Essentials;

namespace EGGs.Droid
{
    public partial class MainActivity:czIActivity
    {


        private App cfCreate_APP()
        {
            //Android.Support.V4.Content.FileProvider
            //Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");

            //var a = new App(this); 
            //App._GetUriForFile = AndroidX.Core.Content.FileProvider.GetUriForFile;
            //LoadApplication(a);


            //StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            //StrictMode.SetVmPolicy(builder.Build());
            ////builder.DetectFileUriExposure();
            //////StrictMode.disableDeathOnFileUriExposure
            ///


            Path_APP = GetExternalFilesDir(null).AbsolutePath;
            Path_Cache = GetExternalCacheDirs().Any() ? GetExternalCacheDirs().First().AbsolutePath : null;
            Path_Download = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;

            //var x1 = MediaStore.Downloads.ExternalContentUri;
            //var x2 = MediaStore.Downloads.InternalContentUri;

            //if ((Build.VERSION.SdkInt) < BuildVersionCodes.P)//[9 28 P] [10 29 Q]
            //if (((int)Build.VERSION.SdkInt) < 29) Path_Download = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
            //else
            //{

            //    Path_Download1 = MediaStore.AuthorityUri.EncodedPath;
            //    Path_Download2 = MediaStore.MediaScannerUri.EncodedPath;
            //    Path_Download3 = MediaStore.Files.GetContentUri("external").EncodedPath;
            //    Path_Download4 = MediaStore.Images.Media.ExternalContentUri.EncodedPath;
            //    Path_Download5 = MediaStore.Images.Media.InternalContentUri.EncodedPath;
            //}

            return new App(this);
        }




        //var cxP = cxMainActivity.FilesDir.AbsolutePath; ///data/data/TiViAl.OAO/files

        public string Path_APP { get; private set; }
        public string Path_Cache { get; private set; }
        public string Path_Download { get; private set; }




        private void cfFunc()
        {
            //FilesDir
            //AndroidEnvironment. ExternalStorageDirectory?.CanonicalPath,
        }





        public void cfSet_UnhandledExceptionRaiser(Action<Exception,string> cxFunction, Func<bool> cxHandled)
        {
            AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                cxFunction(e.Exception, "AndroidEnvironment.UnhandledExceptionRaiser");
                e.Handled = cxHandled();
            };
        }

        public void cfShutDown()
        {
            Process.KillProcess(Process.MyPid());
        }










        public void cfToast(string cxText, bool cxLength_Long)
        {
            var cxT = Toast.MakeText(Application.Context, cxText, cxLength_Long?ToastLength.Long:ToastLength.Short);
            //cxT.SetGravity(GravityFlags.Top, 0, 0);
            cxT.Show();
        }



        ////Miss Click popup 
        //cxAD.CancelEvent += (s, e) =>
        //{

        //};

        public async Task<int?> cfAlertDialog(string cxText, string cxTitle, string cxYes = "Да", string cxNo = "Нет", string cxCancel = "Отмена")
        {
            int? cxR = null;
            if (this != null)
            {
                bool isHide = false;
                AlertDialog.Builder cxb = new AlertDialog.Builder(this);
                cxb.SetTitle(cxTitle);
                cxb.SetMessage(cxText);
                if (cxYes != null) cxb.SetPositiveButton(cxYes, (s, e) => cxR = 1);
                if (cxNo != null) cxb.SetNegativeButton(cxNo, (s, e) => cxR = 2);
                if (cxCancel != null) cxb.SetNeutralButton(cxCancel, (s, e) => cxR = 0);
                AlertDialog cxAD = cxb.Create();

                //Every time when close (any button or missclick)
                cxAD.DismissEvent += (s, e) => isHide = true;
                cxAD.Show();

                await Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(300);
                    } while (!isHide);
                });
            }
            return cxR;
        }



        public async Task<DateTime?> cfDateDialog(DateTime cxDateStart, DateTime? cxbtn2 = null, DateTime? cxbtn3 = null, string cxTitle = null, string cxText = null)
        {
            DateTime? cxR = null;
            bool cxHide = false;
            if (this != null)
            {
                string cxFormat = "dd.MM.yy";
                DatePickerDialog cxDD = new DatePickerDialog(this, cfEventHandler, cxDateStart.Year, cxDateStart.Month - 1, cxDateStart.Day);

                if (cxTitle != null) cxDD.SetTitle(cxTitle);
                if (cxText != null) cxDD.SetMessage(cxText);

                cxDD.SetButton("OK", (s, e) => cxR = cxDD.DatePicker.DateTime);
                if (cxbtn2 != null) cxDD.SetButton2(cxbtn2.Value.ToString(cxFormat), (s, e) => cxR = cxbtn2);
                if (cxbtn3 != null) cxDD.SetButton3(cxbtn3.Value.ToString(cxFormat), (s, e) => cxR = cxbtn3);

                cxDD.DismissEvent += (s, e) => cxHide = true;
                //cxDD.CancelEvent += (s, e) =>
                //{
                //    //missclick
                //};

                cxDD.Show();

                await Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(300);
                    } while (!cxHide);
                });
            }
            return cxR;


            void cfEventHandler(object sender, DatePickerDialog.DateSetEventArgs e)
            {
                //cxR = new DateTime(e.Year, e.Month+1, e.DayOfMonth); 
            }


        }




        public Task<TimeSpan?> cfTimeDialog(TimeSpan cxTimeStart, TimeSpan? cxbtn2 = null, TimeSpan? cxbtn3 = null, string cxTitle = null, string cxText = null)
        {
            if (this == null) throw new Exception("cfTimeDialog _mainActivity == null");
            czTimePickerDialog_Ex cxTD = new czTimePickerDialog_Ex(this, true);
            return cxTD.cfShow(cxTimeStart, cxbtn2, cxbtn3, cxTitle, cxText);
        }








        //public void cfRun_APK2(string cxFile)
        //{
        //    Intent install = new Intent(Intent.ActionInstallPackage);

        //    File cxFile_java = new File(cxFile);
        //    Android.Net.Uri cxUri = Android.Net.Uri.FromFile(cxFile_java);
        //    //cfMSG_Info(((int)Build.VERSION.SdkInt).ToString());
        //    //if (((int)Build.VERSION.SdkInt) < 24)
        //    //{
        //    //    cxUri = Android.Net.Uri.FromFile(cxFile_java);
        //    //}
        //    //else
        //    //{
        //    //    cxUri = Android.Net.Uri.Parse(cxFile_java.Path); // My work-around for new SDKs, worked for me in Android 10 using Solid Explorer Text Editor as the external editor.
        //    //}


        //    install.SetDataAndType(cxUri, "application/vnd.android.package-archive");
        //    install.SetFlags(ActivityFlags.NewTask);
        //    //install.AddFlags(ActivityFlags.GrantReadUriPermission);
        //    _mainActivity.StartActivity(Intent.CreateChooser(install, "Your title"));
        //}




        //public void cfRun_APK(string cxFile)
        //{
        //    File cxFile_java = new File(cxFile);

        //    Android.Net.Uri cxUri = App._GetUriForFile(_mainActivity,"TiViAl.OAO.provider", cxFile_java);

        //    Intent install = new Intent(Intent.ActionView);
        //    install.SetDataAndType(cxUri, "application/vnd.android.package-archive");
        //    install.SetFlags(ActivityFlags.NewTask);
        //    install.AddFlags(ActivityFlags.GrantReadUriPermission);
        //    _mainActivity.StartActivity(install);
        //}





        //public void cfRun_APK(string cxFile)
        //{
        //    File cxFile_java = new File(cxFile);


        //    var x1= Android.Net.Uri.FromFile(cxFile_java);
        //    var x2= Android.Net.Uri.Parse(cxFile_java.Path);


        //    //cfRUN(x1);
        //    cfRUN(x2);



        //    //Android.Net.Uri cxUri=x2;



        //    //Intent install = new Intent(Intent.ActionView);
        //    //install.SetDataAndType(cxUri, "application/vnd.android.package-archive");
        //    //install.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
        //    //install.AddFlags(ActivityFlags.GrantReadUriPermission);
        //    //_mainActivity.StartActivity(install);


        //    void cfRUN(Android.Net.Uri cxUri)
        //    {
        //        //if (((int)Build.VERSION.SdkInt) >= 24)
        //        //{
        //        //    try
        //        //    {
        //        //        StrictMode.VmPolicy.
        //        //    }
        //        //    catch (Exception)
        //        //    {

        //        //        throw;
        //        //    }
        //        //}


        //        Intent install = new Intent(Intent.ActionView);
        //        install.SetDataAndType(cxUri, "application/vnd.android.package-archive");
        //        install.SetFlags(ActivityFlags.NewTask);
        //        //install.AddFlags(ActivityFlags.GrantReadUriPermission);
        //        _mainActivity.StartActivity(install);
        //    }


        //}


        //public void cfRun_APK(string cxFile)
        //{
        //    Intent install = new Intent(Intent.ActionInstallPackage);

        //    File cxFile_java = new File(cxFile);
        //    Android.Net.Uri cxUri;
        //    cfMSG_Info(((int)Build.VERSION.SdkInt).ToString());
        //    if (((int)Build.VERSION.SdkInt) < 24)
        //    {
        //        cxUri = Android.Net.Uri.FromFile(cxFile_java);
        //    }
        //    else
        //    {
        //        cxUri = Android.Net.Uri.Parse(cxFile_java.Path); // My work-around for new SDKs, worked for me in Android 10 using Solid Explorer Text Editor as the external editor.
        //    }
        //    //install.SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(cxFile)),"application/vnd.android.package-archive");
        //    install.SetDataAndType(cxUri, "application/vnd.android.package-archive");
        //    install.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
        //    install.AddFlags(ActivityFlags.GrantReadUriPermission);
        //    _mainActivity.StartActivity(install);
        //}

        //public void cfRun_Package(string cxPackage_Name)
        //{
        //    PackageManager cxPM = _mainActivity.PackageManager;
        //    Intent install = cxPM.GetLaunchIntentForPackage(cxPackage_Name);
        //    install.SetFlags(ActivityFlags.NewTask);
        //    _mainActivity.StartActivity(install);
        //}


    }
}