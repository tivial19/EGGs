//using System;
//using System.Collections.Generic;
//using System.Text.Json;

//namespace cpADD.Ability.IJSON
//{
//    public class czJson_System : czIJSON
//    {
//        readonly JsonSerializerOptions _opt;
//        public czJson_System()
//        {
//            //_opt=new JsonSerializerOptions() { };
//        }

//        public object cfDeserialize_Object_From_String(string cxText, Type cxType)
//        {
//            return JsonSerializer.Deserialize(cxText,cxType);
//        }

//        public T cfDeserialize_Object_From_String<T>(string cxText)
//        {
//            return JsonSerializer.Deserialize<T>(cxText);
//        }



//        public string cfSerialize_ObjectToString<T>(T cxObject)
//        {
//            return JsonSerializer.Serialize(cxObject);
//        }


//    }
//}
