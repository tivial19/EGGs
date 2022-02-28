using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.Ability
{
    public interface czISaverOPT
    {
        string cfGet_Param_String(object cxModel, string cxKey, string cxDefault_Value = null);
        void cfSet_Param_String(object cxModel, string cxKey, string cxValue);


        T cfGet_Param_Convert<T>(object cxModel, string cxKey, T cxDefault_Value = default(T));
        void cfSet_Param_Convert<T>(object cxModel, string cxKey, T cxValue);


        T cfGet_Param_Object<T>(object cxModel, string cxKey = null, T cxDefault_Value = default);
        void cfSet_Param_Object<T>(object cxModel, T cxObject, string cxKey = null);


        Task cfSave_File();
        



        //Task<(bool, T)> cfGet_Param<T>(string cxKey = null, string cxCollection = null) where T : class;
        //Task cfInsert_Param<T>(T cxObject, string cxCollection = null);
        //Task cfInsert_Param<T>(T cxObject, string cxKey, string cxCollection = null);





        //string cfGet_Param(string cxKey);
        //string cfGet_Param(object cxModel_Key);

        //T cfGet_Param_Object<T>(string cxKey); 
        //T cfGet_Param_Object<T>(object cxModel_Key);



        //void cfSet_Param(string cxKey, string cxValue);
        //void cfSet_Param(object cxModel_Key, string cxValue);

        //void cfSet_Param<T>(object cxModel_Key, T cxObject_Value);
        //void cfSet_Param<T>(string cxKey, T cxObject_Value);
        //void cfSet_Param<T>(T cxObject_Value);
        ////T cfGet_Param_Anonimus<T>(string cxKey, T cxAnonimus_Object) where T : class;
        ////T cfGet_Param_Anonimus<T>(object cxModel_Key, T cxAnonimus_Object) where T : class;
    }

}




//T cfGet_Object_Anonim<T>(T cxAnonim)
//{
//    return _Save.cfGet_Param_Object<T>(this);
//}