using System;
using System.Collections.Generic;
using System.Text;

namespace cpADD
{
    public class czAPP_Exception_Normal : Exception
    {
        public string Title { get; }


        public czAPP_Exception_Normal(string cxText= "Действие отменено!", string cxTitle="Внимание") :base(cxText)
        {
            Title=cxTitle;
        }



    }




    public class czAPP_Exception_Convertor : Exception
    {
        public czAPP_Exception_Convertor(string cxText) : base(cxText)
        {

        }
    }

}
