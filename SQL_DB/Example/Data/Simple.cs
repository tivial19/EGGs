using SQL_DB.Data;

using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_DB.Example
{
    public class czSimple_Data
    {
        public czSimple_Data()
        {

        }


        [czaPrimaryKey(true)]
        public int id { get; set; }


        [czaNotNull]
        public string Text { get; set; }



    }
}
