using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cpADD.Ability.ISerializer
{


    public class czSerializer_JSON : czISerializer
    {
        Encoding cvEncoding { get; }

        czIJSON _json;


        public czSerializer_JSON(czIJSON cxjson)
        {
            cvEncoding = Encoding.UTF8;
            _json = cxjson;
        }



        public T cfDeserialize_Object_From_File<T>(string cxFile)
        {
            if (File.Exists(cxFile))
            {
                string cxS = cfGet_String_From_File(cxFile);
                return _json.cfDeserialize_Object_From_String<T>(cxS);
            }
            else return default(T);
        }

        public object cfDeserialize_Object_From_File(string cxFile, Type cxType)
        {
            if (File.Exists(cxFile))
            {
                string cxS = cfGet_String_From_File(cxFile);
                return _json.cfDeserialize_Object_From_String(cxS, cxType);
            }
            else return default(object);
        }

        public void cfSerialize_ObjectToFile(string cxFile, object cxObject)
        {
            File.WriteAllText(cxFile, _json.cfSerialize_ObjectToString(cxObject), cvEncoding);
        }






        private string cfGet_String_From_File(string cxFile)
        {
            return File.ReadAllText(cxFile, cvEncoding);
        }
    }
}
