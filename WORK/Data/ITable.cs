using System;
using System.Collections.Generic;
using System.Text;

namespace cpWORK
{


    //public enum ceEGG_Group_Enum
    //{
    //    Стая1 = 1, Стая2 = 2
    //}



    public interface czITable_Data
    {
        long id { get; set; }
        long Date { get; set; }

        int Count_1 { get; set; }
        int Count_2 { get; set; }

        decimal Money { get; set; }

        string Comment { get; set; }


    }
}
