using System;
using System.Threading.Tasks;

namespace SQL_DB
{

    //public interface czITable_Sql<T>: czITable_Sql_Base
    //{
    //    Task<T[]> cfLoad_All();
    //}



    public interface czITable_Sql_Base
    {
        string Table_Name { get; }
        Type Data_Type { get; }

        Task<int> cfClear();
        //Task<int> cfClear_trunc();
        Task<int> cfCount();
        Task<int> cfDrop();
        string[] cfGet_Data_Fields();
        string cfGet_Primary_Key();
    }



}