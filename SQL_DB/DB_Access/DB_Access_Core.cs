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

        public string cfGet_Tables_Colums_text()
        {
            var cxData = cfGet_Tables_Colums();
            List<string> cxL = new List<string>();
            foreach (var d in cxData) 
            {
                cxL.Add($"          Таблица: {d.Table_Name}");
                cxL.AddRange(d.Colums);
            }
            return string.Join(Environment.NewLine, cxL);
        }

        public (string Table_Name, string[] Colums)[] cfGet_Tables_Colums()
        {
            return cfGet_All_Tables().Select(t => (t.Table_Name, t.cfGet_Data_Columns())).ToArray();
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
