using SQL_DB.Data;

using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_DB.Example
{
    public class czItem_Data2
    {

        public czItem_Data2()
        {

        }


        [czaPrimaryKey(true)]
        public int id { get; set; }

        //[czaNotNull]
        [czaForeignKey(nameof(czItem_Data),nameof(id))]
        public int czItem_Data_id { get; set; }

        [czaIgnore]
        public czItem_Data Item { get; set; }


        [czaNotNull]
        [czaDefault("_")]
        //[czaCheck("!='' && !=' '")]
        public string Text { get; set; }



    }
}
