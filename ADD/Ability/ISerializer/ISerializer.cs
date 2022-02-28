using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.Ability
{

    public interface czISerializer
    {
        void cfSerialize_ObjectToFile(string cxFile, object cxObject);

        object cfDeserialize_Object_From_File(string cxFile, Type cxType);
        T cfDeserialize_Object_From_File<T>(string cxFile);

        // Save Dictionary<string, object>, load IEnumerable<TypeOfObjects>
        //IEnumerable<object> cfDeserialize_DictionaryObjects_From_File(string cxFile, IEnumerable<Type> cxTypesOfObjects);
        
    }








}
