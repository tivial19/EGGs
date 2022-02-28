using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB
{
    //WARNING T1 must be type of table. T2, T3 and else is link tables with forgen keys
    public class czTable_Sql_Join<T1, T2> : czTable_Sql_Base<T1> where T1 : class where T2 : class
    {
        readonly czTable_Sql_Base<T2> _Table_Join;
        readonly Func<T1, T2, T1> _Func_Join;

        public czTable_Sql_Join(czTable_Sql_Base<T2> cxTable_Join, Func<T1, T2, T1> cxFunc_Join, Func<IDbConnection> cxConn, string cxName = null) : base(cxConn, cxName)
        {
            _Table_Join=cxTable_Join;
            _Func_Join=cxFunc_Join;
        }




        //public async Task<T[]> cfLoad_Join(czITable_Sql cxTable_Join)
        //{
        //    string cxFormat = "Select * From {0} Join {2} on {0}.{1} = {2}.{3}";
        //    string cxField_Forgn = cfGet_Foreign_Key(cxTable_Join.Table_Name);
        //    string cxField_Primary = cxTable_Join.cfGet_Primary_Key();

        //    string cxCmd = string.Format(cxFormat, Table_Name, cxField_Forgn, cxTable_Join.Table_Name, cxField_Primary);
        //    var Qr = await _conn.QueryAsync<T>(cxCmd);

        //    throw new NotImplementedException();
        //    //return Qr.ToArray();
        //}

        public async Task<T1[]> cfLoad_Join() 
        {
            string cxFormat = "Select * From [{0}] T1 Join [{2}] T2 on T1.{1} = T2.{3}";
            string cxField_Forgn = cfGet_Foreign_Key(_Table_Join.Table_Name);
            string cxField_Primary = _Table_Join.cfGet_Primary_Key();

            string cxCmd = string.Format(cxFormat, Table_Name, cxField_Forgn, _Table_Join.Table_Name, cxField_Primary);
            //var Qr = await _conn.QueryAsync<T1, T2, T1>(cxCmd, _Join_Func);
            var Qr = await _conn().QueryAsync(cxCmd, _Func_Join);
            return Qr.ToArray();
        }




    }
}
