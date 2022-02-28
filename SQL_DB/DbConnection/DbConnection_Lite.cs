using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using SQL_DB.DbConnection;

namespace SQL_DB
{
    public class czDbConnection_Lite : czDbConnection_Core
    {

        //public czDbConnection_Lite(IDbConnection cxConn) : base(cxConn)
        //{

        //}

        public czDbConnection_Lite(Func<string, IDbConnection> cxGet_conn, string cxDataBase) : base(cxGet_conn,"Data Source={0}",cxDataBase,cxDataBase,ceDB_SQL_Type.SQLite)
        {

        }

        public override Task cfCreate_DB()
        {
            return Task.Delay(1);//create table is enof
        }

        public async override Task<bool> cfisTable_Exists(string cxTable_Name)
        {
            if (await cfisDB_Exists())
            {
                string cxCmd = $"Select Count (*) from sqlite_master where name = '{cxTable_Name}' AND type='table'";
                var cxR = await cfExecute_CMD_Scalar(cxCmd);
                return Convert.ToInt32(cxR)==1;
            }
            else return false;
        }



    }

}
