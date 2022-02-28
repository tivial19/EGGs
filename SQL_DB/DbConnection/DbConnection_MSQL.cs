using SQL_DB.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB
{
    public class czDbConnection_MSQL : czDbConnection_MSQL_Common
    {
        const string ccServer_def = "TIVIAL\\SQLEXPRESS";
        const string ccConnection_Format = "Server={0};Database={1};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        //const string cxformat = "Data Source={0};Initial Catalog={1};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //public czDbConnection_MSQL(IDbConnection cxConn) : base(cxConn)
        //{
        //}

        public czDbConnection_MSQL(Func<string, IDbConnection> cxGet_conn, string cxDataBase, string cxServer = null) : base(cxGet_conn, cfGet_Connection_String_Static(cfGet_Server(cxServer), cxDataBase), cfGet_Server(cxServer), cxDataBase, ceDB_SQL_Type.MSQL)
        {

        }

        private static string cfGet_Connection_String_Static(string cxServer, string cxDataBase)
        {
            return string.Format(ccConnection_Format, cxServer, cxDataBase);
        }

        private static string cfGet_Server(string cxServer)
        {
            return string.IsNullOrWhiteSpace(cxServer) ? ccServer_def : cxServer;
        }

        protected override string cfGet_Connection_String(string cxDataBase)
        {
            return cfGet_Connection_String_Static(Server, cxDataBase);
        }





        public async override Task cfCreate_DB()
        {
            await cfCreate_DB_MSQL(DataBase);
        }

        public override Task<bool> cfisDB_Exists()
        {
            return cfisDB_Exists(DataBase);
        }










        private async Task<bool> cfisDB_Exists(string cxDB_Name)
        {
            string cxConnString = cfGet_Connection_String(ccMaster_Table);
            string cxCmd = $"Select count(*) from master.dbo.sysdatabases where name='{cxDB_Name}'";
            var cxR = await cfExecute_CMD_Scalar_in_New_Connection(cxConnString, cxCmd);
            return Convert.ToInt32(cxR)==1;
        }

        private async Task cfCreate_DB_MSQL(string cxDB_Name)
        {
            bool cxisExs = await cfisDB_Exists(cxDB_Name);
            if(cxisExs==false)
            {
                string cxConnString = cfGet_Connection_String(ccMaster_Table);
                string cxCmd = $"CREATE DATABASE {cxDB_Name}";
                await cfExecute_CMD_in_New_Connection(cxConnString, cxCmd);
            }
        }








    }
}
