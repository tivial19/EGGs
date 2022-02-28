using SQL_DB.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace cpWORK
{

    //[NotNull]
    //[czaUniqueOne]
    ////[czaUniqueGroup]
    ///
    public class czTable_Data: czITable_Data
    {

       
        public czTable_Data()
        {
            id = DateTime.Now.Ticks;
            Date = DateTime.Today.Ticks;
            Count_1 = 0;
            Count_2 = 0;
            Money = 0;
        }

        [czaPrimaryKey(false)]
        public long id { get; set; }

        [czaNotNull]
        [czaUniqueOne]
        public long Date { get; set; }


        [czaNotNull]
        public int Count_1 { get; set; }
        [czaNotNull]
        public int Count_2 { get; set; }
        [czaNotNull]
        public decimal Money { get; set; }

        public string Comment { get; set; }


    }








}
