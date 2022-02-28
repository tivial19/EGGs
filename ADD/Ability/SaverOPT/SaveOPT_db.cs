//using cpADD.APP;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace cpADD.Ability.SaverOPT
//{

//    [czaAutoRegisterSingleInstance]
//    public class czSaverOPT : czISaverOPT
//    {
//        Dictionary<string, string> Params { get;  set; }

//        czIDB_Doc _Db;
//        czIAPP_Info _APP_Info;

//        string cvFile_Params=null;



//        public czSaverOPT(czIAPP_Info cxAPP_Info, czIDB_Doc cxDb)
//        {
//            _APP_Info = cxAPP_Info;
//            _Db = cxDb;
//            cfSet_File();
//            _Db.cfInject<czaAuto_Ctor>(cvFile_Params);
//        }




//        public Task cfInsert_Param<T>(T cxObject, string cxCollection = null)
//        {
//            throw new Exception("cfInsert_Param");
//            //return _Db.cfUpdate_Vaue<T>(cxObject, typeof(T).Name, cxCollection);
//        }

//        public Task cfInsert_Param<T>(T cxObject, string cxKey, string cxCollection = null)
//        {
//            throw new Exception("cfInsert_Param");
//            //return _Db.cfUpdate_Vaue<T>(cxObject, cxKey, cxCollection);
//        }



//        public Task<(bool,T)> cfGet_Param<T>(string cxKey=null, string cxCollection = null) where T : class
//        {
//            cxKey ??= typeof(T).Name;
//            return _Db.cfLoad_Vaue<T>(cxKey,cxCollection);
//        }






//        private void cfSet_File()
//        {
//            if(cvFile_Params==null)
//            {
//                string cxName = "NoName";
//                if (!string.IsNullOrWhiteSpace(_APP_Info.Package_Name)) cxName = _APP_Info.Name;
//                if (!string.IsNullOrWhiteSpace(_APP_Info.Name)) cxName = _APP_Info.Name;
//                cvFile_Params = System.IO.Path.Combine(_APP_Info.Path_App, cxName + ".ini");
//            }
//        }



//    }


//}
