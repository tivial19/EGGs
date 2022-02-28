using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cpADD.APP
{

    public class czAPP_Info_Create_Struct 
    { 
        public string Path_App, Path_Cache, Path_Download, Name="NoName", Version="unknown", Package_Name= "noPackege", Device_Name = "noDevice";
    }




    public class czAPP_Info: czIAPP_Info
    {
        Stopwatch cvStart_Timer;


        public czAPP_Info(czAPP_Info_Create_Struct cxCreate_Info)
        {
            cvStart_Timer = Stopwatch.StartNew();

            //string cxP1 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);          //[/data/data/TiViAl.OAO/files/] [C:\\Users\\TiViAl_User\\Documents]
            //string cxP11 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);      //[][]
            //string cxP1 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);       //[][[C:\\Users\\TiViAl_User]]
            //string cxP2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);   //[/data/data/TiViAl.OAO/files/.config] [C:\\Users\\TiViAl_User\\AppData\\Roaming]
            
            if (string.IsNullOrWhiteSpace(cxCreate_Info.Path_App)) throw new Exception("czAPP_Info: Path_App cannot be null!");
            Path_App = cxCreate_Info.Path_App;
            Path_Cache = string.IsNullOrWhiteSpace(cxCreate_Info.Path_Cache) ? System.IO.Path.Combine(Path_App, "Cache") : cxCreate_Info.Path_Cache;
            Path_Download = string.IsNullOrWhiteSpace(cxCreate_Info.Path_Download) ? System.IO.Path.Combine(Path_App, "Download") : cxCreate_Info.Path_Download;

            const string cxCore = "_Core";
            if (cxCreate_Info.Name?.EndsWith(cxCore)??false) Name = cxCreate_Info.Name.Substring(0, cxCreate_Info.Name.Length - cxCore.Length);
            else Name = cxCreate_Info.Name;
            Package_Name = cxCreate_Info.Package_Name;
            Version = cxCreate_Info.Version;
            Device_Name = cxCreate_Info.Device_Name;

            Title = Name + " " + Version;
            Shared_PRJ_Info = cfGet_PRJ_Shared_Info();

            var Qa = GetType().FullName.Split(".");
            if (Qa.Any()) Assembly_Class_Name=Qa.First();
            else Assembly_Class_Name=GetType().FullName;
        }

        public czAPP_Info(string cxPath_App) : this(new czAPP_Info_Create_Struct() { Path_App=cxPath_App}) {}
        public czAPP_Info(string cxPath_App, string cxName, string cxVersion) : this(new czAPP_Info_Create_Struct() { Path_App=cxPath_App, Name = cxName, Version=cxVersion }){}
        public czAPP_Info(string cxPath_App, string cxName, string cxVersion, string cxPackage_Name, string cxDevice_Name) : this(new czAPP_Info_Create_Struct() { Path_App = cxPath_App, Name= cxName, Version = cxVersion, Package_Name = cxPackage_Name, Device_Name = cxDevice_Name }){}





        public TimeSpan Time_from_Start => cvStart_Timer.Elapsed;


        public string Year => "2021";

        public string Path_App { get; }
        public string Path_Cache { get; }
        public string Path_Download { get; }


        public string Name { get; }
        public string Package_Name { get; }
        public string Version { get; }
        public string Device_Name { get; }
        public string Title { get; }
        public string[] Shared_PRJ_Info { get; }

        public string Assembly_Class_Name { get; }

        private string[] cfGet_PRJ_Shared_Info()
        {
            List<string> cxL = new List<string>();
            var QV = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.FullName.Contains(nameof(czStatic_Info)));
            if (QV.Any())
            {
                string cxPropName = nameof(czStatic_Info.Version);
                foreach (var cxItem in QV)
                {
                    var cxP = cxItem.GetProperty(cxPropName);
                    if (cxP != null) cxL.Add(cxP.GetValue(this).ToString());
                }
            }
            return cxL.ToArray();
        }
    }
}
