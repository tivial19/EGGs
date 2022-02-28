using SQL_DB.DB_Access;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB.Example
{
    public class czDB_Simple : czDB_Access_Core
    {
        public czDB_Simple(czIDbConnection conn) : base(conn)
        {
            tb_Simple=new czTable_Sql_Base<czSimple_Data>(_Conn.cfGet_Connection, "Million");
        }


        public czTable_Sql_Base<czSimple_Data> tb_Simple { get; }



    }
}
