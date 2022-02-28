using SQL_DB.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB
{
    public class czDbConnection_MSQLocal : czDbConnection_MSQL_Common
    {
        const string ccServer = "(LocalDB)\\MSSQLLocalDB";
        const string ccConnection_Format = "Data Source={0};AttachDbFilename={1};Integrated Security=False;Connect Timeout=30;Encrypt=False;Trusted_Connection=True;";


        public czDbConnection_MSQLocal(Func<string, IDbConnection> cxGet_conn, string cxDataBase) : base(cxGet_conn,cfGet_Connection_String_Static(ccServer,cxDataBase), ccServer,cxDataBase, ceDB_SQL_Type.MSQLocal)
        {

        }


        private static string cfGet_Connection_String_Static(string cxServer, string cxDataBase)
        {
            return string.Format(ccConnection_Format, cxServer, cxDataBase);
        }

        protected override string cfGet_Connection_String(string cxDataBase)
        {
            return cfGet_Connection_String_Static(Server, cxDataBase);
        }






        public async override Task cfCreate_DB()
        {
            await cfCreate_DB_MSQLocal(DataBase);
        }






        private async Task cfCreate_DB_MSQLocal(string cxFile_DB)
        {
            string cxDataBase_Name = Path.GetFileNameWithoutExtension(cxFile_DB);
            string cxFile_ldf = Path.Combine(Path.GetDirectoryName(cxFile_DB), cxDataBase_Name +".ldf");

            if (!File.Exists(cxFile_DB))
            {
                string cxConn = $"Data Source ={ccServer}; database=master";
                string cxCmd_format =
                        "CREATE DATABASE {0} ON PRIMARY " +
                        "(NAME = {0}_Data, " +
                        "FILENAME = '{1}', " +
                        "SIZE = 3MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
                        "LOG ON (NAME = {0}_Log, " +
                        "FILENAME = '{2}', " +
                        "SIZE = 1MB, " +
                        "MAXSIZE = 5MB, " +
                        "FILEGROWTH = 10%)";

                string cxCmd = string.Format(cxCmd_format, cxDataBase_Name, cxFile_DB, cxFile_ldf);

                await cfExecute_CMD_in_New_Connection(cxConn, cxCmd);
            }
        }


    }
}
