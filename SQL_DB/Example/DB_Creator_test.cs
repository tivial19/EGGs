using SQL_DB.DB_Access;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB.Example
{

    public interface czIDB_Creator_Test
    {
        Task cfCreate_DB();
        czTable_Sql_Base<czTable_For_Create> tbTest { get; }
    }



    public class czDB_Create : czDB_Access_Core, czIDB_Creator_Test 
    {

        public czDB_Create(czIDbConnection conn) : base(conn)
        {
            tbTest =new czTable_Sql_Base<czTable_For_Create>(_Conn.cfGet_Connection);
        }

        public czTable_Sql_Base<czTable_For_Create> tbTest { get;}


        public Task cfCreate_DB()
        {
            return cfCreate_DB_and_AllTables();
        }

    }









    //public class czDB_Create_Lite : czDB_Create_Common
    //{
    //    public czDB_Create_Lite(Func<string, IDbConnection> cxGet_conn, string cxDataBase) : base(new czDbConnection_Lite(cxGet_conn, cxDataBase))
    //    {

    //    }

    //}




    //public class czDB_Create_MS_local : czDB_Create_Common
    //{
    //    public czDB_Create_MS_local(Func<string, IDbConnection> cxGet_conn, string cxDataBase) : base(new czDbConnection_MSQLocal(cxGet_conn, cxDataBase))
    //    {

    //    }
    //}

    //public class czDB_Create_MS_Server : czDB_Create_Common
    //{

    //    public czDB_Create_MS_Server(Func<string, IDbConnection> cxGet_conn, string cxDataBase, string cxServer = null):base(new czDbConnection_MSQL(cxGet_conn, cxDataBase, cxServer))
    //    {

    //    }
    //}







}
