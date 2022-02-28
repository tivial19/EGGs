using cpADD.APP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.Ability.SaverOPT
{

    [czaAutoRegisterSingleInstance]
    public class czSaverOPT : czISaverOPT
    {
        const string cvSimbol_Equal = "=";
        Encoding cvEncoding = Encoding.UTF8;

        czIJSON _Json;
        czIAPP_Info _APP_Info;

        string cvFile_Params=null;
        Dictionary<string, string> Params { get; set; }


        public czSaverOPT(czIJSON cxJson, czIAPP_Info cxAPP_Info)
        {
            _Json = cxJson;
            _APP_Info = cxAPP_Info;
            cfSet_File();
            cfLoad_File(cvFile_Params);



            void cfSet_File()
            {
                if (cvFile_Params == null)
                {
                    string cxName = "NoName";
                    if (!string.IsNullOrWhiteSpace(_APP_Info.Package_Name)) cxName = _APP_Info.Name;
                    if (!string.IsNullOrWhiteSpace(_APP_Info.Name)) cxName = _APP_Info.Name;
                    cvFile_Params = System.IO.Path.Combine(_APP_Info.Path_App, cxName + ".ini");
                }
            }




            void cfLoad_File(string cxFile)
            {
                if (File.Exists(cxFile))
                {
                    string[] cxParams = File.ReadAllLines(cxFile, cvEncoding);

                    if (cxParams.Any())
                    {
                        Params = new Dictionary<string, string>();
                        int cxIndex = 0;
                        string cxKey, cxValue;
                        foreach (string cxLine in cxParams)
                        {
                            cxIndex = cxLine.IndexOf(cvSimbol_Equal);
                            cxKey = cxLine.Substring(0,cxIndex);
                            cxValue = cxLine.Substring(cxIndex+1,cxLine.Length - (1 + cxIndex));
                            Params.Add(cxKey, cxValue);
                        }
                    }
                }
                //else throw new Exception($"Файл не найден {cxFile}");
            }


        }



        private string cfGet_Key(object cxModel, string cxKey)
        {
            return $"{cxModel.GetType().Name}.{cxKey}";
        }




        public Task cfSave_File()
        {
            return Task.Run(() =>
            {
                if (Params?.Any() ?? false)
                {
                    List<string> cxArray = new List<string>();
                    foreach (string cxKey in Params.Keys) cxArray.Add($"{cxKey}{cvSimbol_Equal}{Params[cxKey]}");
                    File.WriteAllLines(cvFile_Params, cxArray.OrderBy(s=>s).ToArray(), cvEncoding);
                }
            });
        }



        public string cfGet_Param_String(object cxModel, string cxKey, string cxDefault_Value=null)
        {
            if (Params?.Any() ?? false)
            {
                if(Params.TryGetValue(cfGet_Key(cxModel, cxKey), out string cxValue)) return cxValue;
            }
            return cxDefault_Value;
        }

        public T cfGet_Param_Convert<T>(object cxModel, string cxKey, T cxDefault_Value=default(T))
        {
            string cxS = cfGet_Param_String(cxModel, cxKey, null);
            if (cxS == null) return cxDefault_Value;
            else return (T)Convert.ChangeType(cxS, typeof(T));
        }

        public T cfGet_Param_Object<T>(object cxModel, string cxKey= null, T cxDefault_Value = default(T))
        {
            if (cxKey == null) cxKey = typeof(T).Name;
            string cxS = cfGet_Param_String(cxModel, cxKey, null);
            if (cxS == null) return cxDefault_Value;
            else return _Json.cfDeserialize_Object_From_String<T>(cxS);
        }




        public void cfSet_Param_String(object cxModel, string cxKey, string cxValue)
        {
            if (Params == null) Params = new Dictionary<string, string>();
            string cxKey_full = cfGet_Key(cxModel,cxKey);
            if (!Params.ContainsKey(cxKey_full)) Params.Add(cxKey_full, cxValue);
            else Params[cxKey_full] = cxValue;
            
            cfSave_File();
        }

        public void cfSet_Param_Convert<T>(object cxModel, string cxKey, T cxValue)
        {
            cfSet_Param_String(cxModel, cxKey, Convert.ToString(cxValue));
        }

        public void cfSet_Param_Object<T>(object cxModel, T cxObject, string cxKey= null)
        {
            if (cxKey == null) cxKey = cxObject.GetType().Name;
            cfSet_Param_String(cxModel, cxKey, _Json.cfSerialize_ObjectToString(cxObject));
        }






        //public string cfGet_Param(string cxKey)
        //{
        //    if(Params!=null)
        //    {
        //        if (Params.TryGetValue(cxKey, out string cxValue)) return cxValue;
        //    }
        //    return null;
        //}
        //public string cfGet_Param(object cxModel_Key)
        //{
        //    return cfGet_Param(cxModel_Key.GetType().Name);
        //}



        //public T cfGet_Param_Object<T>(string cxKey) //where T : class
        //{
        //    string cxValue = cfGet_Param(cxKey);
        //    if (cxValue != null) return _Json.cfDeserialize_Object_From_String<T>(cxValue);
        //    return default(T);
        //}

        //public T cfGet_Param_Object<T>(object cxModel_Key) //where T : class
        //{
        //    return cfGet_Param_Object<T>(cxModel_Key.GetType().Name);
        //}



        ////public T cfGet_Param_Anonimus<T>(string cxKey, T cxAnonimus_Object) where T : class
        ////{
        ////    return cfGet_Param_Object<T>(cxKey);
        ////}

        ////public T cfGet_Param_Anonimus<T>(object cxModel_Key, T cxAnonimus_Object) where T : class
        ////{
        ////    return cfGet_Param_Object<T>(cxModel_Key.GetType().Name);
        ////}






        //public void cfSet_Param(string cxKey, string cxValue)
        //{
        //    if (Params == null) Params = new Dictionary<string, string>();
        //    if (!Params.ContainsKey(cxKey)) Params.Add(cxKey, cxValue);
        //    else Params[cxKey] = cxValue;
        //}

        //public void cfSet_Param(object cxModel_Key, string cxValue)
        //{
        //    cfSet_Param(cxModel_Key.GetType().Name, cxValue);
        //}




        //public void cfSet_Param<T>(string cxKey, T cxObject_Value)
        //{
        //    cfSet_Param(cxKey, _Json.cfSerialize_ObjectToString<T>(cxObject_Value));
        //}
        //public void cfSet_Param<T>(object cxModel_Key, T cxObject_Value)
        //{
        //    cfSet_Param<T>(cxModel_Key.GetType().Name, cxObject_Value);
        //}

        //public void cfSet_Param<T>(T cxObject_Value)
        //{
        //    cfSet_Param<T>(cxObject_Value.GetType().Name, cxObject_Value);
        //}




    }


}
