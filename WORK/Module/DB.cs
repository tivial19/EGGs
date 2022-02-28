using cpADD;
using cpADD.Ability;
using cpWORK.Enties;
using Microsoft.Data.Sqlite;
using SQL_DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cpWORK
{



    public class czDB : czDB_Access_Core
    {
        czTable_Sql_Base<czTable_Data> tbl_Main { get; }


        public string File => _Conn.Server;


        czISerializer _Serilizer;
        readonly string ccFile_json;

        public czDB(czIAPP_Info cxApp, czISerializer cxSer) : base(cfGet_Connection(cxApp))
        {
            tbl_Main=new czTable_Sql_Base<czTable_Data>(_Conn.cfGet_Connection, "Main");
            _Serilizer=cxSer;
            ccFile_json = Path.Combine(cxApp.Path_App, Path.GetFileNameWithoutExtension(File)+".txt");
        }



        static czIDbConnection cfGet_Connection(czIAPP_Info cxApp)
        {
            string cvFileExE = ".sqlite";
            string cxFile = Path.Combine(cxApp.Path_App, cxApp.Name + cvFileExE);
            czIDbConnection cxDBC = new czDbConnection_Lite((s) => new SqliteConnection(s), cxFile);
            return cxDBC;
        }

        private czItem[] cfConvert_Data(IEnumerable<czTable_Data> cxdata)
        {
            return cxdata.Select(d => new czItem(d)).ToArray();
        }

        public Task cfCreate_Tables()
        {
            return cfCreate_DB_and_AllTables();
        }


        public async Task<czItem[]> cfLoad_ALL()
        {
            var Q = await tbl_Main.cfLoad_All();
            return Q.Select(d => new czItem(d)).ToArray();
        }

        public Task cfInsert_Row(czItem cxRow)
        {
            return tbl_Main.cfInsert(cxRow._data);
        }

        //public Task cfInsert_Rows(IEnumerable<czTable> cxRows)
        //{
        //    return _conn.InsertAllAsync(cxRows);
        //}

        public Task cfReplace_Row(czItem cxRow)
        {
            return tbl_Main.cfUpdate(cxRow._data);
        }

        public Task cfDelete_Row(czItem cxRow)
        {
            return tbl_Main.cfDelete(cxRow._data);
        }





        public async Task<czItem[]> cfLoad_Filter(czFilter cxFilter, bool cxAdd_Start, bool cxAdd_End, bool isUse_Where_directly, czTable_Data cxTable = null)
        {
            if (cxFilter.isEmpty) return await cfLoad_ALL();
            if (isUse_Where_directly) return cfConvert_Data(await tbl_Main.cfLoad_Where(cxFilter.Text));

            List<string> cxQuery_Where_And = new List<string>();

            if (!string.IsNullOrWhiteSpace(cxFilter.Text)) cxQuery_Where_And.Add(cfGet_Where_Colums_Words_AND(cxFilter.Text.Split(" ",StringSplitOptions.RemoveEmptyEntries), cfGet_Columns(), cxAdd_Start, cxAdd_End));
            if (cxFilter.isDate_Used) cxQuery_Where_And.Add(cfGet_Date_Query(nameof(cxTable.Date), cxFilter.Date_End, cxFilter.Date_Start));



            if (cxQuery_Where_And.Any())
            {
                string cxCmd = string.Join(" AND ", cxQuery_Where_And);
                var Q = await tbl_Main.cfLoad_Where(cxCmd);
                return cfConvert_Data(Q);
            }
            else return await cfLoad_ALL();






            string[] cfGet_Columns(czTable_Data cxTable = null)
            {
                return new string[] { nameof(cxTable.Count_1), nameof(cxTable.Count_2), nameof(cxTable.Money), nameof(cxTable.Comment) };
            }


            static string cfGet_Where_Colums_Words_AND(string[] cxWords, string[] cxCols, bool cxAdd_Start, bool cxAdd_End)
            {
                var Q = cxWords.Select(w => cfGet_Where_Colums_Word_OR(w, cxCols, cxAdd_Start, cxAdd_End));
                string r = string.Join(" AND ", Q);
                return "(" + r + ")";
            }

            static string cfGet_Where_Colums_Word_OR(string cxWord, string[] cxCols, bool cxAdd_Start, bool cxAdd_End)
            {
                //("Comment" like '%55%' or "Name" like '%35%')
                string cxFormat_one_col = "\"{0}\" LIKE '{2}{1}{3}'";
                var C = cxCols.Select(c => string.Format(cxFormat_one_col, c, cxWord, cxAdd_Start ? "%" : string.Empty, cxAdd_End ? "%" : string.Empty));
                string r = string.Join(" OR ", C);
                return "(" + r + ")";
            }

            static string cfGet_Date_Query(string cxField_Name, DateTime cxDate_End, DateTime cxDate_Start)
            {
                if (cxDate_End <= cxDate_Start) return $"{cxField_Name} = {cxDate_End.Ticks}";
                else return $"{cxField_Name} >= {cxDate_Start.Ticks} AND {cxField_Name} <= {cxDate_End.Ticks}";
            }

        }






        public async Task cfBack_UP_json(czItem[] cxItems)
        {
            czTable_Data[] cxData = null;
            if (cxItems?.Any()??false) cxData = cxItems.Select(i => i._data).ToArray();
            else cxData = await tbl_Main.cfLoad_All();

            if (cxData?.Any()??false) _Serilizer.cfSerialize_ObjectToFile(ccFile_json, cxData);
            else throw new czAPP_Exception_Normal("Данные для резерва пусты!");
        }

        public async Task<czItem[]> cfRestore_json(bool cxAdd)
        {
            if (System.IO.File.Exists(ccFile_json))
            {
                var Q = _Serilizer.cfDeserialize_Object_From_File<czTable_Data[]>(ccFile_json);
                if (Q.Any())
                {
                    if (!cxAdd) await tbl_Main.cfClear();
                    await tbl_Main.cfInsert_Quick(Q);
                    Q = await tbl_Main.cfLoad_All();
                    return Q.Select(d => new czItem(d)).ToArray();
                }
                else throw new czAPP_Exception_Normal($"Список резерва пуст!");
            }
            else throw new czAPP_Exception_Normal($"Файл не найден: {ccFile_json}");
        }




        public string[] cfGet_Data_Fields()
        {
            var cxProps = typeof(czTable_Data).GetProperties(BindingFlags.Public |  BindingFlags.Instance);
            return cxProps.Select(p => p.Name).ToArray();
        }




    }
}
