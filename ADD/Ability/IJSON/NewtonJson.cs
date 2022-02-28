//#define HAVE_Newtonsoft
//#if HAVE_Newtonsoft

//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace cpADD.Ability.IJSON
//{


//    public class czJson_Newton : czIJSON
//    {
//        JsonSerializerSettings cvSettings_Deserialize;

//        public czJson_Newton()
//        {
//            cvSettings_Deserialize = new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace };// Collection=values. In Auto Collection.add(values)
//        }



//        public string cfSerialize_ObjectToString<T>(T cxObject)
//        {
//            return JsonConvert.SerializeObject(cxObject);

//            //Type cxObject_Type = cxObject.GetType();
//            //Type cxInterface_Type = typeof(T);

//            //if (cxObject_Type == cxInterface_Type) return JsonConvert.SerializeObject(cxObject);
//            //else
//            //{
//            //    //return JsonConvert.SerializeObject(cxObject);
//            //    if (!cxInterface_Type.IsInterface || !cxObject_Type.IsClass) throw new Exception("cfSerialize_ObjectToString InterfaceContractResolver Wrong Types");
//            //    IContractResolver cxContractResolver = new InterfaceContractResolver(cxObject_Type, cxInterface_Type);
//            //    return JsonConvert.SerializeObject(cxObject, cxInterface_Type, new JsonSerializerSettings() { ContractResolver = cxContractResolver });
//            //}
//        }



//        public T cfDeserialize_Object_From_String<T>(string cxText)
//        {
//            return JsonConvert.DeserializeObject<T>(cxText, cvSettings_Deserialize);
//        }

//        public object cfDeserialize_Object_From_String(string cxText, Type cxType)
//        {
//            return JsonConvert.DeserializeObject(cxText, cxType, cvSettings_Deserialize);
//        }


//    }



//    //public class czJson_Newton:czIJSON
//    //{
//    //    protected JsonSerializer cvJS;

//    //    JsonSerializerSettings cvSettings_Deserialize;

//    //    public Type Attribute => typeof(czaAutoSaveAttribute);


//    //    public czJson_Newton()
//    //    {
//    //        cvSettings_Deserialize = new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace };// Collection=values. In Auto Collection.add(values)

//    //        cvJS = new JsonSerializer();
//    //        cvJS.ObjectCreationHandling = cvSettings_Deserialize.ObjectCreationHandling;
//    //    }



//    //    public string cfSerialize_ObjectToString(object cxObject)
//    //    {
//    //         return JsonConvert.SerializeObject(cxObject,new JsonSerializerSettings() { ContractResolver = new czJSON_ContractResolver(Attribute) });
//    //    }





//    //    public T cfDeserialize_Object_From_String<T>(string cxText)
//    //    {
//    //         return JsonConvert.DeserializeObject<T>(cxText,cvSettings_Deserialize);
//    //    }

//    //    public object cfDeserialize_Object_From_String(string cxText, Type cxType)
//    //    {
//    //        return JsonConvert.DeserializeObject(cxText, cxType ,cvSettings_Deserialize);
//    //    }


//    //}



//    //public class czJSON_ContractResolver : DefaultContractResolver
//    //{

//    //    Type _Attribute;


//    //    public czJSON_ContractResolver(Type cxAttribute)
//    //    {
//    //        _Attribute = cxAttribute;
//    //    }



//    //    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
//    //    {
//    //        var Qp = base.CreateProperties(type, memberSerialization);
//    //        var Qr = Qp.Where(s => s.AttributeProvider.GetAttributes(_Attribute, true).Any());
//    //        return Qr.ToList();
//    //    }


//    //}


//}


//#endif
