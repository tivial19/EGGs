using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB.DbConnection
{
    public class czDbConnection_MSQL_Common : czDbConnection_Core
    {
        public const string ccMaster_Table = "master";

        public czDbConnection_MSQL_Common(Func<string, IDbConnection> cxGet_conn, string cxConnection_String_Format, string cxServer, string cxDataBase, ceDB_SQL_Type cxDB_Type) : base(cxGet_conn, cxConnection_String_Format, cxServer, cxDataBase, cxDB_Type)
        {

        }

        protected virtual string cfGet_Connection_String(string cxDataBase)
        {
            throw new Exception("czDbConnection_MSQL_Common have no this information");
        }




        public async override Task<bool> cfisTable_Exists(string cxTable_Name)
        {
            if (await cfisDB_Exists())
            {
                string cxConnString = cfGet_Connection_String(DataBase);
                string cxCmd = $"Select Count (*) from information_schema.tables where table_name = '{cxTable_Name}'";
                var cxR = await cfExecute_CMD_Scalar_in_New_Connection(cxConnString, cxCmd);
                return Convert.ToInt32(cxR)==1;
            }
            else return false;
        }


        public async override Task<string> cfGet_Server_Info()
        {
            if (await cfisDB_Exists())
            {
                string cxConnString = cfGet_Connection_String(DataBase);
                string cxCmd = $"SELECT @@VERSION AS 'SQL Server PDW Version'";
                var cxR = await cfExecute_CMD_Scalar_in_New_Connection(cxConnString, cxCmd);
                return cxR.ToString();
            }
            else return "There is no server information!";
        }





    }
}
