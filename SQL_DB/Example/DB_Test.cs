using Dapper;
using SQL_DB.DB_Access;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB.Example
{

    public class czDB_Test_Common: czDB_Access_Core 
    {

        public czDB_Test_Common(czIDbConnection conn) : base(conn)
        {
            tbItems=new czTable_Test(_Conn.cfGet_Connection);

            tbItems2=new czTable_Sql_Join<czItem_Data2, czItem_Data>(tbItems, (t1, t2) => { t1.Item=t2; return t1; }, _Conn.cfGet_Connection);
        }

        //public readonly czTable_Test tbItems;
        public czTable_Test tbItems { get; }
        public czTable_Sql_Join<czItem_Data2,czItem_Data> tbItems2 { get; }








        //public async Task<czItem_Data2[]> cfLoad_Join()
        //{
        //    string cxFormat = "Select * From [{0}] T1 Join [{2}] T2 on T1.{1} = T2.{3}";
        //    string cxField_Forgn = tbItems2.cfGet_Foreign_Key(tbItems.Table_Name);
        //    string cxField_Primary = tbItems.cfGet_Primary_Key();

        //    string cxCmd = string.Format(cxFormat, tbItems2.Table_Name, cxField_Forgn, tbItems.Table_Name, cxField_Primary);
        //    var Qr = await _Conn.Connection.QueryAsync<czItem_Data2, czItem_Data, czItem_Data2>(cxCmd, (i2, i1) => { i2.Item=i1; return i2; });

        //    return Qr.ToArray();
        //}










        //public IDbConnection Connection => _Conn.Connection;

        //public string Connection_String => _Conn.Connection_String;

        //public ceDB_SQL_Type DB_Type => _Conn.DB_Type;

        //public string Server => _Conn.Server;

        //public string DataBase => _Conn.DataBase;

        //public void cfAdd_Table(czITable_Sql cxTable)
        //{
        //    _Conn.cfAdd_Table(cxTable);
        //}

        //public Task cfCreate_DB()
        //{
        //    return _Conn.cfCreate_DB();
        //    //await tbTest.cfCreate();
        //}

        //public Task<string> cfGet_Server_Info()
        //{
        //    return _Conn.cfGet_Server_Info();
        //}

        //public Task<bool> cfisDB_Exists()
        //{
        //    return _Conn.cfisDB_Exists();
        //}

        //public Task<bool> cfisTable_Exists(string cxTable_Name)
        //{
        //    return _Conn.cfisTable_Exists(cxTable_Name);
        //}
    }



    //public class czDB_Test_Lite:czDbConnection_Lite
    //{

    //    public czDB_Test_Lite(Func<string, IDbConnection> cxGet_conn, string cxDataBase) : base(cxGet_conn, cxDataBase)
    //    {
    //        //string[] cxParams_Table_Items = new string[]
    //        //{
    //        //    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE",
    //        //    "Value INTEGER NOT NULL",
    //        //    "Text TEXT NOT NULL"
    //        //};

    //        tbItems=new czTable_Test(this);
    //        tbItems2=new czTable_Sql<czItem_Data2>(this);
    //    }


    //    public czTable_Test tbItems { get; }


    //    public czTable_Sql<czItem_Data2> tbItems2 { get;  }


    //}
}
