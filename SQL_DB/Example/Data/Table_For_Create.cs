using SQL_DB.Data;

using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_DB.Example
{

    public enum ceTable_Test_Enum
    {
        где_то = 0, погреб = 1, коридор = 2, кухня = 3, ларь = 4
    }


    [czaUniqueMany(nameof(Int_V),nameof(Int16_V))]
    //[czaUniqueMany(nameof(Int32_V), nameof(Int64_V))]
    [czaCheckMany("{0}+{1}=10", nameof(Int_V),nameof(Int16_V))]
    public class czTable_For_Create
    {
        public czTable_For_Create()
        {

        }

        [czaPrimaryKey(true)]
        public int id { get; set; }


        [czaIgnore]
        public bool isActive { get; set; }

        [czaNotNull]
        public int Int_V { get; set; }




        public Int16 Int16_V { get; set; }

        [czaUniqueOne]
        public Int32 Int32_V { get; set; }

        public Int64 Int64_V { get; set; }

        [czaNull]
        public byte byte_V { get; set; }

        public double Double_F { get; set; }

        [czaCheckOne("{0}>0")]
        public decimal Decimal_F { get; set; }


        public float? Float_F { get; set; }

        [czaNotNull]
        public Single? Single_F { get; set; }

        [czaDefault("_")]
        public string Text { get; set; }


        public ceTable_Test_Enum enums { get; set; }


        public DateTime Data { get; set; }


    }
}
