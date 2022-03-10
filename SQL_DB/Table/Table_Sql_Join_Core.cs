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



        protected Task<T0[]> cfLoad_ALL_Join<T1>(Func<T0, T1, T0> cxFunc_Join)
        {
            return cfLoad_Where_Join<T1>(string.Empty, cxFunc_Join);
        }


        protected async Task<T0[]> cfLoad_Where_Join<T1>(string cxWhere, Func<T0, T1, T0> cxFunc_Join)
        {
            var cxT0_Foreigns = cfGet_Foreigns_Keys(Tables_Foreign_Name, 1);
            string cxCmd = cfGet_Select_Join_cmd(Table_Name, cxT0_Foreigns, cxWhere);
            var Qr = await _conn().QueryAsync(cxCmd, cxFunc_Join);
            return Qr.ToArray();
        }





        protected Task<T0[]> cfLoad_ALL_Join<T1, T2>(Func<T0, T1, T2, T0> cxFunc_Join)
        {
            return cfLoad_Where_Join<T1,T2>(string.Empty, cxFunc_Join);
        }


        protected async Task<T0[]> cfLoad_Where_Join<T1, T2>(string cxWhere, Func<T0, T1, T2, T0> cxFunc_Join)
        {
            var cxT0_Foreigns = cfGet_Foreigns_Keys(Tables_Foreign_Name, 2);
            string cxCmd = cfGet_Select_Join_cmd(Table_Name, cxT0_Foreigns, cxWhere);
            var Qr = await _conn().QueryAsync(cxCmd, cxFunc_Join);
            return Qr.ToArray();
        }













        protected czForeign_Data[] cfGet_Foreigns_Keys(string[] cxTables_Foreign_Names, int cxCount_Need)
        {
            if (cxTables_Foreign_Names?.Any()??false && cxTables_Foreign_Names.Length==cxCount_Need)
            {
                var Qf = cxTables_Foreign_Names.Select(s => cfGet_Foreign_Key_Id(s)).ToArray();
                czForeign_Data[] cxR = new czForeign_Data[cxCount_Need];
                for (int i = 0; i < cxTables_Foreign_Names.Length; i++)
                {
                    var cxForeign = cfGet_Foreign_Key_Id(cxTables_Foreign_Names[i]);
                    cxR[i]= new czForeign_Data(cxTables_Foreign_Names[i],cxForeign.Foreign_Key,cxForeign.Foreign_Id);
                }
                return cxR;
            }
            else throw new Exception("czTable_Sql_Join_Core cfLoad_Where_Join cxTables_Foreign empty or wrong format");
        }





        static string cfGet_Select_Join_cmd(string cxTable_Name, czForeign_Data[] cxForeign_Data, string cxWhere)
        {
            string cxCmd = $"Select * From [{cxTable_Name}] T0 ";
            string cxJoins = cfGet_Joins_cmd(cxForeign_Data);
            cxCmd+=cxJoins;
            if (!string.IsNullOrWhiteSpace(cxWhere)) cxCmd+=$" Where {cxWhere}";
            return cxCmd;
        }

        static string cfGet_Joins_cmd(czForeign_Data[] cxForeign_Data)
        {
            // join [Foreign_Table_Name] on [Foreign_Key]=[Foreign_Table_Id]
            // T0 T1 T2 
            const string ccTable_Class_Start = "T";
            const string ccTable_Class_Main = ccTable_Class_Start + "0";
            string cxTable_Class_Foreign;

            string[] cxJoins = new string[cxForeign_Data.Length];

            for (int i = 0; i < cxForeign_Data.Length; i++)
            {
                cxTable_Class_Foreign=ccTable_Class_Start+(i+1).ToString();//first cxTable_Class_Foreign="T1"
                cxJoins[i]=cfGet_Join(cxForeign_Data[i].Foreign_Table_Name, cxForeign_Data[i].Foreign_Key, cxForeign_Data[i].Foreign_Table_Id, ccTable_Class_Main, cxTable_Class_Foreign); 
            }
            return string.Join(" ", cxJoins);


            static string cfGet_Join(string cxForeign_Table_Name, string cxMain_Table_Forgn_Key, string cxForeign_Table_id, string cxMain_Table_Class, string cxForeign_Table_Class)
            {
                string cxFormat = "Join [{0}] {4} on {3}.{1} = {4}.{2}";
                return string.Format(cxFormat, cxForeign_Table_Name, cxMain_Table_Forgn_Key, cxForeign_Table_id, cxMain_Table_Class, cxForeign_Table_Class);
            }
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
