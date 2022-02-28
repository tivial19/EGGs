using System;
using System.Collections.Generic;
using System.Text;

namespace cpADD
{


    public class czStringText : czNotify_Object
    {
        public czStringText()
        {

        }

        public czStringText(string cxText)
        {
            Text = cxText;
        }

        public string Text { get; set; }
    }




    public class czStringText2 : czNotify_Object
    {

        public czStringText2()
        {

        }

        public czStringText2(string cxText1, string cxText2)
        {
            Text1 = cxText1;
            Text1 = cxText2;
        }

        public string Text1 { get; set; }
        public string Text2 { get; set; }
    }






}
