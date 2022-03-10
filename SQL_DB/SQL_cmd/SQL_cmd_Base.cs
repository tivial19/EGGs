using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQL_DB.SQL_cmd
{
    public class czSQL_cmd_Base: czSQL_cmd_Core
    {
        protected const string op_AND = " AND ";
        protected const string op_OR = " OR ";



        public czSQL_cmd_Base()
        {

        }



        public string cfGet_Where_Colum_Word(string cxColumn_Name, string cxWord, bool cxAdd_Start, bool cxAdd_End)
        {
            return string.Format("\"{0}\" LIKE '{2}{1}{3}'", cxColumn_Name, cxWord, cxAdd_Start ? "%" : string.Empty, cxAdd_End ? "%" : string.Empty);
        }





        protected string cfGet_Where_Colum_Words_ORAND(string cxColumn_Name, string[] cxWord, bool cxAdd_Start, bool cxAdd_End, bool cxORAND)
        {
            //("Comment" like '%35%' or(and) "Comment" like '%35%')
            var cxLikes = cxWord.Select(w => cfGet_Where_Colum_Word(cxColumn_Name, w, cxAdd_Start, cxAdd_End));
            string r = string.Join(cxORAND ? op_AND : op_OR, cxLikes);
            return "(" + r + ")";
        }

        //and all colums the same => not need on practic
        protected string cfGet_Where_Colums_Word_ORAND(string[] cxCols, string cxWord, bool cxAdd_Start, bool cxAdd_End, bool cxORAND=false)
        {
            //("Comment" like '%35%' or "Name" like '%35%')
            var cxLikes = cxCols.Select(c => cfGet_Where_Colum_Word(c, cxWord, cxAdd_Start, cxAdd_End));  
            string r = string.Join(cxORAND ? op_AND : op_OR, cxLikes);
            return "(" + r + ")";
        }




        //AND default; OR also can be Used (any column like any word)
        protected string cfGet_Where_Colums_Words_ORAND(string[] cxCols, string[] cxWords, bool cxAdd_Start, bool cxAdd_End, bool cxORAND)
        {
            //("Comment" like '%35%' or "Name" like '%35%') AND(OR) ("Comment" like '%9%' or "Name" like '%9%')
            var cxWhere = cxWords.Select(w => cfGet_Where_Colums_Word_ORAND(cxCols, w, cxAdd_Start, cxAdd_End,false));
            string r = string.Join(cxORAND ? op_AND : op_OR, cxWhere);
            return "(" + r + ")";
        }









    }
}
