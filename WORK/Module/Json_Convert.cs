using cpADD.Ability;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cpWORK.Module
{
    
    
    
    
    public class czJson_Convert
    {


        czIJSON _json;

        string _file_json;

        public czJson_Convert(string cxFile_json, czIJSON cxjson)
        {
            _json = cxjson;
            _file_json = cxFile_json;

            cfRead_Biils();
        }

        public List<czTable_Data> Items { get; set; }



        private void cfRead_Biils()
        {
            string cxF = File.ReadAllText(_file_json, Encoding.UTF8);
            Items = _json.cfDeserialize_Object_From_String<List<czTable_Data>>(cxF);
        }








    }
















    //class czBill
    //{
    //    public long Ticks { get; set; }
    //    IEnumerable<czBill_Item> Items { get; }
    //}


    //class czBill_Item
    //{
    //    decimal Count_Items_or_Packages { get; set; }
    //    decimal? Count_Value_in_Package { get; set; }
    //    czItem_Source Item_Source { get; set; }
    //}

    //class czItem_Source
    //{
    //    public czItem_Source()
    //    {

    //    }

    //    public string[] Params { get; set; }

    //}





}
