﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB.DB_Access.DB_Creator
{
    public class czDB_Table_Creator
    {

        public czDB_Table_Creator(czIDbConnection conn)
        {
            _Conn=conn;
        }


        protected czIDbConnection _Conn { get; }

        public ceTable_SQL_Type Tables_type => cfGet_Table_Type(_Conn.DB_Type);





        public Task<int> cfCreate_Table(string cxCMD_Create)
        {
            return _Conn.cfExecute_CMD(cxCMD_Create);
        }

        public async Task<int> cfCreate_Table(string cxTable_Name, string[] cxParams_Create)
        {
            if (true == await _Conn.cfisTable_Exists(cxTable_Name)) return 0;
            czTable_Cmd_Create_Core cxTable_cmd_Create =new czTable_Cmd_Create_Core();
            string cxCMD_Create = cxTable_cmd_Create.cfGet_Create_Cmd(cxTable_Name, cxParams_Create);
            return await cfCreate_Table(cxCMD_Create);
        }


        public async Task<int> cfCreate_Table(string cxTable_Name, Type cxTable_Type)
        {
            if (true == await _Conn.cfisTable_Exists(cxTable_Name)) return 0;
            czTable_Cmd_Create_Base cxTable_cmd_Create = new czTable_Cmd_Create_Base(Tables_type);
            string cxCMD_Create = cxTable_cmd_Create.cfGet_Create_Cmd(cxTable_Type,cxTable_Name);
            return await cfCreate_Table(cxCMD_Create);
        }









        protected async Task cfCreate_Tables_ALL()
        {
            var cxProps = GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var Q = cxProps.Select(p => cfis_Generic_Type_Base(p, typeof(czTable_Sql_Base<>), this)).Where(t => t.OK).ToArray();

            if (Q.Any())
            {
                foreach (var cxtbl in Q) await cfCreate_Table(cxtbl.Name,cxtbl.Data_Type);
            }
            else throw new Exception("cfCreate_Tables_ALL. Thre is no tables");


            static (bool OK, Type Data_Type, string Name) cfis_Generic_Type_Base(PropertyInfo cxPI, Type cxGeneric_Type_Definition, object cxThis)
            {
                Type cxType = cxPI.PropertyType;
                bool cxR = false;
                do
                {
                    if (cfis_Types_Equal(cxType, cxGeneric_Type_Definition)) cxR=true;
                    else cxType=cxType.BaseType;

                } while (cxR==false && cxType!=null);


                if (cxR)
                {
                    Type t = cxType.GetGenericArguments()[0];
                    return (true, t, cfGet_Table_Name(cxPI, cxThis));
                }
                else return (false, null, null);




                static bool cfis_Types_Equal(Type cxType_Check, Type cxGeneric_Type_Definition)
                {
                    if (cxType_Check.IsGenericType) return cxType_Check.GetGenericTypeDefinition()==cxGeneric_Type_Definition;
                    else return false;
                }


                static string cfGet_Table_Name(PropertyInfo cxPI, object cxThis)
                {
                    return ((czTable_Sql_Core)cxPI.GetValue(cxThis)).Table_Name;
                }


            }

        }





        //protected async Task cfCreate_Tables_ALL()
        //{
        //    var cxTables_Class = cfGet_Tables_Type<czITable_create>(this);
        //    if ((cxTables_Class?.Any()??false)==false) throw new Exception("cfCreate_Tables_ALL. Thre is no tables");

        //    foreach (var cxTable in cxTables_Class) await cfCreate_Table(cxTable);



        //    static I[] cfGet_Tables_Type<I>(object cxThis) //where I: interface
        //    {
        //        //var cxProps = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //        //var Q1= cxProps.Where(p => p.PropertyType.IsGenericType).Where(p => typeof(czTable_Sql<>)==p.PropertyType.GetGenericTypeDefinition());
        //        //var Q2 = cxProps.Where(p => p.PropertyType.BaseType.IsGenericType).Where(p => typeof(czTable_Sql<>)==p.PropertyType.BaseType.GetGenericTypeDefinition());
        //        //var Q = Q1.Concat(Q2).ToArray();

        //        var cxProps = cxThis.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType.GetInterface(typeof(I).Name)!=null).ToArray();

        //        if (cxProps.Any()) return cxProps.Select(p => p.GetValue(cxThis)).Cast<I>().ToArray();
        //        else return null;
        //    }

        //}







        



        private ceTable_SQL_Type cfGet_Table_Type(ceDB_SQL_Type cxDB_Type)
        {
            switch (cxDB_Type)
            {
                case ceDB_SQL_Type.SQLite:
                    return ceTable_SQL_Type.SQLite;

                case ceDB_SQL_Type.MSQL:
                case ceDB_SQL_Type.MSQLocal:
                    return ceTable_SQL_Type.MSQL;

                default:
                    break;
            }

            throw new Exception("czTable_Sql cfGet_Table_Type have no any parametr");
        }




    }
}
