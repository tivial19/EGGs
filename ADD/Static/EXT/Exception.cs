using System;
using System.Collections.Generic;
using System.Text;

namespace cpADD.EXT
{
    public static class czext_Exception
    {

        public static string cfGet_Message_Exception(this Exception cxE)
        {
            List<string> cxL = new List<string>();
            cxL.Add(cfGet_msg(cxE));
            if (cxE.InnerException != null)
            {
                Exception cxIE = cxE.InnerException;
                do
                {
                    cxL.Add(cfGet_msg(cxIE));
                    cxIE = cxIE.InnerException;
                } while (cxIE != null);
            }
            return string.Join(Environment.NewLine, cxL);

            static string cfGet_msg(Exception cxE)
            {
                return "$" + cxE.GetType().Name + ". " + cxE.Source + ": " + Environment.NewLine + cxE.Message;
            }
        }


    }
}
