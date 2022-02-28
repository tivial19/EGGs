using System;
using System.Collections.Generic;
using System.Text;
using Utf8Json;

namespace cpADD.Ability.IJSON
{
    public class czJson_UTF8 : czIJSON
    {
        public czJson_UTF8()
        {

        }

        public object cfDeserialize_Object_From_String(string cxText, Type cxType)//????????????????????????????????????????
        {
            throw new NotImplementedException();
        }



        public T cfDeserialize_Object_From_String<T>(string cxText)
        {
            return JsonSerializer.Deserialize<T>(cxText);
        }

        public string cfSerialize_ObjectToString<T>(T cxObject)
        {
            return JsonSerializer.ToJsonString<T>(cxObject);
        }



        //public T cfDeserialize_Anonimus_Object_From_String<T>(string cxText, T cxAnonimus_Object)
        //{
        //    return cfDeserialize_Object_From_String<T>(cxText);
        //}

    }
}
