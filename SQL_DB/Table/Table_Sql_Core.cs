using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB
{
    public class czTable_Sql_Core
    {
        public string Table_Name { get; }

        protected readonly Func<IDbConnection> _conn;

        public czTable_Sql_Core(Func<IDbConnection> cxConn, string cxName)
        {
            _conn = cxConn;
            Table_Name=cxName;
            if (string.IsNullOrWhiteSpace(cxName)) throw new Exception("czTable_Sql_Core Table_Name empty");
        }


  
        protected Task<int> cfCMD_Execute(string cxSql_cmd, object cxParam = null)
        {
            return _conn().ExecuteAsync(cxSql_cmd, cxParam);
        }


        protected Task<T> cfCMD_Execute_Scalar<T>(string cxSql_cmd, object cxParam = null)
        {
            return _conn().ExecuteScalarAsync<T>(cxSql_cmd, cxParam);
        }


        protected Task<T> cfQUERY_Single<T>(string cxSql_cmd, object cxParam = null)
        {
            return _conn().QuerySingleAsync<T>(cxSql_cmd, cxParam);
        }

        protected async Task<T[]> cfQUERY<T>(string cxSql_cmd, object cxParam = null)
        {
            var Qr = await _conn().QueryAsync<T>(cxSql_cmd, cxParam);
            return Qr.ToArray();
        }













        //protected Task<int> cfCMD_ExecuteNonQuery(string cxSql_cmd)
        //{
        //    return Task.Run<int>(() =>
        //    {
        //        using (IDbConnection cc = _get_conn())
        //        {
        //            var cmd = cc.CreateCommand();
        //            cmd.CommandText=cxSql_cmd;
        //            cc.Open();
        //            int r = cmd.ExecuteNonQuery();
        //            //cc.Close();
        //            return r;
        //        }
        //    });
        //}

    }
}



//DynamicParameters parameters = new DynamicParameters();
//parameters.Add("Param2", "TheCode");
//parameters.Add("Param3", "TheTitle");
//parameters.Add("Param4", 4);
//parameters.Add("Param5", "2018-01-28");
//parameters.Add("Param6", true);
//parameters.Add("Param7", false);
//parameters.Add("Param8", 300);
//parameters.Add("Param9", 30);
//parameters.Add("Param10", 3);
//parameters.Add("Param11", "2018-01-28");
//parameters.Add("Param12", true);
//parameters.Add("Param13", true);

//var insertedId = await connection.ExecuteAsync(query, parameters, transaction);