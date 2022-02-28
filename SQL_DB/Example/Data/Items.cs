using SQL_DB.Data;

using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_DB.Example
{



    public class czItem_Data
    {

        public czItem_Data()
        {

        }

        public czItem_Data(int value, string text)
        {
            Value = value;
            Text = text;
        }

        public czItem_Data(int id, int value, string text)
        {
            this.id=id;
            Value=value;
            Text=text;
        }

        [czaPrimaryKey(true)]
        public int id { get; set; }

        [czaCheckOne("{0}>0")]
        [czaNotNull]
        //[czaUnique]
        [czaDefault(1)]
        public int Value { get; set; }
        
        //[czaNotNull]
        //[czaDefault("_")]
        public string Text { get; set; }


    }



}
