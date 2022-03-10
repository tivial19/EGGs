using SQL_DB.SQL_cmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQL_DB
{
    public class czSQL_cmd: czSQL_cmd_Base, czISQL_cmd_Create
    {
        public czSQL_cmd()
        {

        }


        public string cfGet_Where_Date(string cxColumn_Name, DateTime cxDate_End, DateTime cxDate_Start)
        {
            if (cxDate_End <= cxDate_Start) return $"{cxColumn_Name} = {cxDate_End.Ticks}";
            else return $"{cxColumn_Name} >= {cxDate_Start.Ticks} AND {cxColumn_Name} <= {cxDate_End.Ticks}";
        }

        public bool cfisAll_words_int(string[] cxWords)
        {
            foreach (var cxW in cxWords)
            {
                if (int.TryParse(cxW, out int cxN)==false) return false;
            }
            return true;
        }
        public string cfGet_Where_IN_Words(string cxColumn_Name, string[] cxWords)
        {
            string cxValues = string.Join(", ", cxWords);
            return $"{cxColumn_Name} IN ({cxValues})";
        }





        public string cfGet_Where_Colum_Words_OR(string cxColumn_Name, string[] cxWords, bool cxAdd_Start, bool cxAdd_End)
        {
            return cfGet_Where_Colum_Words_ORAND(cxColumn_Name, cxWords, cxAdd_Start, cxAdd_End, false);
        }

        public string cfGet_Where_Colum_Words_AND(string cxColumn_Name, string[] cxWords, bool cxAdd_Start, bool cxAdd_End)
        {
            return cfGet_Where_Colum_Words_ORAND(cxColumn_Name, cxWords, cxAdd_Start, cxAdd_End, true);
        }





        public string cfGet_Where_Colums_Word_OR(string[] cxCols, string cxWord, bool cxAdd_Start, bool cxAdd_End)
        {
            return cfGet_Where_Colums_Word_ORAND(cxCols, cxWord, cxAdd_Start, cxAdd_End, false);// TRUE AND on practic not need
        }





        public string cfGet_Where_Colums_Words_AND(string[] cxCols, string[] cxWords, bool cxAdd_Start, bool cxAdd_End)
        {
            return cfGet_Where_Colums_Words_ORAND(cxCols, cxWords, cxAdd_Start, cxAdd_End, true);
        }

        public string cfGet_Where_Colums_Words_OR(string[] cxCols, string[] cxWords, bool cxAdd_Start, bool cxAdd_End)
        {
            return cfGet_Where_Colums_Words_ORAND(cxCols, cxWords, cxAdd_Start, cxAdd_End, false);
        }















    }

}
