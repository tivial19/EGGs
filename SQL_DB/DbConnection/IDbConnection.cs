using System;
using System.Data;
using System.Threading.Tasks;

namespace SQL_DB
{
    public interface czIDbConnection
    {
        ceDB_SQL_Type DB_Type { get; }

        string Connection_String { get; }
        string Server { get; }
        string DataBase { get; }


        IDbConnection cfGet_Connection();

        Task<bool> cfisDB_Exists();
        Task<bool> cfisTable_Exists(string cxTable_Name);
        Task<string> cfGet_Server_Info();



        Task cfCreate_DB();
        Task<int> cfExecute_CMD(string cxCmd, object cxParams = null);
    }



}