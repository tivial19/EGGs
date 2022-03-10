using SQL_DB.DB_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB
{
    public class czDB_Access_Core: czDB_Table_Creator
    {

        public czDB_Access_Core(czIDbConnection conn):base(conn, new czSQL_cmd())
        {
            
        }


        public async Task cfCreate_DB_and_AllTables()
        {
            await _Conn.cfCreate_DB();
            await cfCreate_Tables_ALL();
        }









//From Connection

        public Task<string> cfGet_Server_Info()
        {
            return _Conn.cfGet_Server_Info();
        }

        public Task<bool> cfisDB_Exists()
        {
            return _Conn.cfisDB_Exists();
        }

        public Task<bool> cfisTable_Exists(string cxTable_Name)
        {
            return _Conn.cfisTable_Exists(cxTable_Name);
        }


    }
}
