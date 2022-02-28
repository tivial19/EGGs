using cpADD.APP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.Ability.IMSG
{


    //[czaAutoRegisterIgnore]
    public class czMSG_Output : czIMSG
    {
        public czMSG_Output()
        {

        }

        public void cfMSG(string cxText)
        {
            Console.Out.WriteLine("czIMSG [Console]: " + cxText);
            Debug.WriteLine("czIMSG [Debug]: " + cxText);
        }
    }




    public class czMSG_Output2 : czIMSG2
    {

        czIAPP_MSG _AppMsg;
        public czMSG_Output2(czIAPP_MSG cxAppMsg)
        {
            _AppMsg = cxAppMsg;
        }

        public void cfMSG(string cxText)
        {
            _AppMsg.cfMSG_OK(cxText);
        }
    }


}
