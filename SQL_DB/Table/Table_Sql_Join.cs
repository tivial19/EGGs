using Dapper;
using SQL_DB.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB
{
    //WARNING T0 (main table) must be type of table. T1, T2 and else is link tables with forgen keys

    public class czTable_Sql_Join<T0,T1,T2> : czTable_Sql_Join_Core<T0> where T0 :class where T1 : class where T2 : class
    {
        Func<T0, T1, T2, T0> _Func_Join;

        public czTable_Sql_Join(Func<IDbConnection> cxConn, string cxName, Func<T0, T1, T2, T0> cxFunc_Join, params string[] cxTables_Foreign) : base(cxConn, cxName, cxTables_Foreign)
        {
            _Func_Join=cxFunc_Join;
        }

        public czTable_Sql_Join(Func<IDbConnection> cxConn, Func<T0, T1, T2, T0> cxFunc_Join, params string[] cxTables_Foreign) : this(cxConn, null, cxFunc_Join, cxTables_Foreign)
        {

        }




        public override async Task<T0[]> cfLoad_Join_cmd(string cxCmd)
        {
            var Qr = await _conn().QueryAsync(cxCmd, _Func_Join);
            return Qr.ToArray();
        }





    }











    public class czTable_Sql_Join<T0, T1> : czTable_Sql_Join_Core<T0> where T0 : class where T1 : class
    {
        Func<T0, T1, T0> _Func_Join;

        public czTable_Sql_Join(Func<IDbConnection> cxConn, string cxName, Func<T0, T1, T0> cxFunc_Join, params string[] cxTables_Foreign) : base(cxConn, cxName, cxTables_Foreign)
        {
            _Func_Join=cxFunc_Join;
        }

        public czTable_Sql_Join(Func<IDbConnection> cxConn, Func<T0, T1, T0> cxFunc_Join, params string[] cxTables_Foreign) : this(cxConn, null, cxFunc_Join, cxTables_Foreign)
        {

        }



        public override async Task<T0[]> cfLoad_Join_cmd(string cxCmd)
        {
            var Qr = await _conn().QueryAsync(cxCmd, _Func_Join);
            return Qr.ToArray();
        }




    }
}
