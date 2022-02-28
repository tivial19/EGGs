using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB.DbConnection
{
    public class czDbConnection_Core:czIDbConnection
    {
        readonly Func<string, IDbConnection> _get_conn;
        

        //////for manual access
        //private czDbConnection_Core(IDbConnection cxConn)
        //{
        //    Connection=cxConn;
        //}

        private czDbConnection_Core(Func<string, IDbConnection> cxGet_conn, string cxConnection_String) //:this(cxGet_conn(cxConnection_String))
        {
            _get_conn=cxGet_conn;
            Connection_String=cxConnection_String;
        }

        public czDbConnection_Core(Func<string, IDbConnection> cxGet_conn, string cxConnection_String_Format, string cxServer, string cxDataBase, ceDB_SQL_Type cxDB_Type) : this(cxGet_conn,cfGet_Connection_String(cxConnection_String_Format, cxServer,cxDataBase))
        {
            Server=cxServer;
            DataBase=string.IsNullOrWhiteSpace(cxDataBase)? cxServer:cxDataBase;//for sqlite where datasourse=database
            DB_Type=cxDB_Type;
        }

        private static string cfGet_Connection_String(string cxFormat, string cxServer, string cxDataBase)
        {
            if (string.IsNullOrWhiteSpace(cxServer)) throw new Exception("czDbConnection_Core cfGet_Connection_String Server cannot be null");
            return string.Format(cxFormat, cxServer, cxDataBase);
        }



        //public IDbConnection Connection { get; }

        public string Connection_String { get; }
        public string Server { get; }
        public string DataBase { get; }
        public ceDB_SQL_Type DB_Type { get; }


        //Need for Table
        public IDbConnection cfGet_Connection()
        {
            return _get_conn(Connection_String);
        }


        public virtual Task<string> cfGet_Server_Info()
        {
            return Task.Run<string>(() =>
            {
                return Server;
            });
        }

        public virtual Task<bool> cfisDB_Exists()
        {
            return Task.Run<bool>(() =>
            {
                return File.Exists(DataBase);
            });
        }

        public virtual Task<bool> cfisTable_Exists(string cxTable_Name)
        {
            throw new Exception("cfisTable_Exists cannot used in czDbConnection_Core. Used override function.");
        }


        public virtual Task cfCreate_DB()
        {
            throw new Exception("cfCreate_DB cannot used in czDbConnection_Core. Used override function.");
        }






        public Task<int> cfExecute_CMD(string cxCmd, object cxParams = null)
        {
            var cc = cfGet_Connection();
            return cc.ExecuteAsync(cxCmd,cxParams);
        }




        protected Task<object> cfExecute_CMD_Scalar(string cxCmd, object cxParams=null)
        {
            var cc = cfGet_Connection();
            return cc.ExecuteScalarAsync(cxCmd,cxParams);
        }

        protected Task<int> cfExecute_CMD_in_New_Connection(string cxConn_String, string cxCmd, object cxParams = null)
        {
            var cc = _get_conn(cxConn_String);
            return cc.ExecuteAsync(cxCmd,cxParams);
        }

        protected Task<object> cfExecute_CMD_Scalar_in_New_Connection(string cxConn_String, string cxCmd, object cxParams = null)
        {
            var cc = _get_conn(cxConn_String);
            return cc.ExecuteScalarAsync(cxCmd,cxParams);
        }



    }
}


