using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_DB
{
    public interface czISQL_cmd_Create
    {
        string cfGet_Create_Cmd(string cxTable_Name, string[] cxParams);

        string cfGet_Create_Cmd(ceTable_SQL_Type cxTable_Type, Type cxType, string cxTable_Name = null);
    }

}
