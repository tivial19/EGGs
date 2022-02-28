#define HAVE_Newtonsoft
#if !HAVE_Newtonsoft

using System;
using System.Collections.Generic;
using System.Text;

namespace cpADD.Ability.ISerializer
{
    
    public class czSerializer_txt : czISerializer
    {
        public czSerializer_txt()
        {

        }


        public Type Attribute { get; }

        public IEnumerable<object> cfDeserialize_DictionaryObjects_From_File(string cxFile, IEnumerable<Type> cxTypesOfObjects)
        {
            return null;
        }

        public T cfDeserialize_Object_From_File<T>(string cxFile)
        {
            return default(T);
        }

        public object cfDeserialize_Object_From_File(string cxFile, Type cxType)
        {
            throw new NotImplementedException();
        }

        public void cfSerialize_ObjectToFile(string cxFile, object cxObject)
        {
            
        }
    }
}


#endif