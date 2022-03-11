using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB
{
    public class czTable_Sql_Join_Core<T0> : czTable_Sql_Base<T0> where T0 : class
    {

        readonly string[] Tables_Foreign_Name;


        public czTable_Sql_Join_Core(Func<IDbConnection> cxConn, string cxName, params string[] cxTables_Foreign) : base(cxConn, cxName)
        {
            Tables_Foreign_Name=cxTables_Foreign;
        }

        public czTable_Sql_Join_Core(Func<IDbConnection> cxConn, params string[] cxTables_Foreign) : this(cxConn, null, cxTables_Foreign)
        {

        }


        public virtual Task<T0[]> cfLoad_Join_cmd(string cxCmd)
        {
            throw new NotImplementedException("czTable_Sql_Join_Core have no code cfLoad_Join_cmd USE override in czTable_Sql_Join");
        }



        public Task<T0[]> cfLoad_Join_ALL()
        {
            return cfLoad_Join_cmd(cfGet_cmd_Join());
        }

        public Task<T0[]> cfLoad_Join_Where(string cxWhere)
        {
            return cfLoad_Join_cmd(cfGet_cmd_Join(cxWhere));
        }

        public Task<T0[]> cfLoad_Join_Where_End(string cxWhere, string cxCmd_End)
        {
            return cfLoad_Join_cmd(cfGet_cmd_Join(cxWhere, cxCmd_End));
        }

        public Task<T0[]> cfLoad_Select_Join_Where(string cxSelect, string cxWhere, string cxCmd_End)
        {
            return cfLoad_Join_cmd(cfGet_cmd_Join(cxSelect, cxWhere, cxCmd_End));
        }








        protected string cfGet_cmd_Join(string cxSelect, string cxWhere, string cxEnd)
        {
            string cxJoins = cfGet_Joins_cmd();
            return cfGet_Select_Join_Where_End_cmd(cxSelect, Table_Name, cxJoins, cxWhere, cxEnd);
        }

        protected string cfGet_cmd_Join(string cxWhere, string cxEnd)
        {
            return cfGet_cmd_Join(string.Empty, cxWhere, cxEnd);
        }

        protected string cfGet_cmd_Join(string cxWhere)
        {
            return cfGet_cmd_Join(string.Empty, cxWhere, string.Empty);
        }

        protected string cfGet_cmd_Join()
        {
            return cfGet_cmd_Join(string.Empty, string.Empty, string.Empty);
        }













        private string cfGet_Joins_cmd()
        {
            var cxForeign_Data = cfGet_Foreigns_Keys(Tables_Foreign_Name);
            return cfGet_Joins_cmd(Table_Name, cxForeign_Data);


            czForeign_Data[] cfGet_Foreigns_Keys(string[] cxTables_Foreign_Names)
            {
                if (cxTables_Foreign_Names?.Any()??false)
                {
                    int cxCount_Need = cxTables_Foreign_Names.Length;

                    var Qf = cxTables_Foreign_Names.Select(s => cfGet_Foreign_Key_Id(s)).ToArray();
                    czForeign_Data[] cxR = new czForeign_Data[cxCount_Need];
                    for (int i = 0; i < cxTables_Foreign_Names.Length; i++)
                    {
                        var cxForeign = cfGet_Foreign_Key_Id(cxTables_Foreign_Names[i]);
                        cxR[i]= new czForeign_Data(cxTables_Foreign_Names[i], cxForeign.Foreign_Key, cxForeign.Foreign_Id);
                    }
                    return cxR;
                }
                else throw new Exception("czTable_Sql_Join_Core cfLoad_Where_Join cxTables_Foreign empty or wrong format");
            }

            static string cfGet_Joins_cmd(string cxTable_Main, czForeign_Data[] cxForeign_Data)
            {
                string[] cxJoins = new string[cxForeign_Data.Length];
                for (int i = 0; i < cxJoins.Length; i++)
                    cxJoins[i]=cfGet_Join(cxTable_Main, cxForeign_Data[i].Foreign_Key, cxForeign_Data[i].Foreign_Table_Name, cxForeign_Data[i].Foreign_Table_Id);

                return string.Join(" ", cxJoins);



                static string cfGet_Join(string cxMain_Table_Name, string cxMain_Table_Forgn_Key, string cxForeign_Table_Name, string cxForeign_Table_id)
                {
                    string cxFormat = "Join {2} on {0}.{1} = {2}.{3}";
                    return string.Format(cxFormat, cxMain_Table_Name, cxMain_Table_Forgn_Key, cxForeign_Table_Name, cxForeign_Table_id);
                }
            }
        }





        static string cfGet_Select_Join_Where_End_cmd(string cxSelect, string cxTable_Name, string cxJoin, string cxWhere, string cxEnd)
        {
            if (string.IsNullOrWhiteSpace(cxTable_Name)) throw new Exception("czTable_Sql_Join_Core cfGet_Select_Join_Where_End_cmd cxTable_Name==null");
            if (string.IsNullOrWhiteSpace(cxJoin)) throw new Exception("czTable_Sql_Join_Core cfGet_Select_Join_Where_End_cmd cxJoin==null");

            string cxS = string.IsNullOrWhiteSpace(cxSelect) ? "*" : cxSelect;
            string cxCmd = $"Select {cxS} From {cxTable_Name} {cxJoin}";
            if (!string.IsNullOrWhiteSpace(cxWhere)) cxCmd+=$" Where {cxWhere}";
            if (!string.IsNullOrWhiteSpace(cxEnd)) cxCmd+=" " + cxEnd;
            return cxCmd;
        }






        protected class czForeign_Data
        {
            public czForeign_Data(string foreign_Table_Name, string foreign_Key, string foreign_Table_Id)
            {
                Foreign_Table_Name=foreign_Table_Name;
                Foreign_Key=foreign_Key;
                Foreign_Table_Id=foreign_Table_Id;
            }

            public readonly string Foreign_Table_Name;
            public readonly string Foreign_Key;
            public readonly string Foreign_Table_Id;
        }


    }





}
