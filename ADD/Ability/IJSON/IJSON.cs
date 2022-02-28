using System;
using System.Collections.Generic;
using System.Text;

namespace cpADD.Ability
{
    public interface czIJSON
    {
        string cfSerialize_ObjectToString<T>(T cxObject);       // cfSerialize_ObjectToString<czIData>(cxData) == cfSerialize_ObjectToString(cxIData) => typeof(T)==czIData, Object.getType=czData

        T cfDeserialize_Object_From_String<T>(string cxText);
        object cfDeserialize_Object_From_String(string cxText, Type cxType);

        //T cfDeserialize_Anonimus_Object_From_String<T>(string cxText, T cxAnonimus_Object);
    }


    //public interface czIJSON8: czIJSON
    //{

    //}

}
