using SQL_DB.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQL_DB.DB_Access.DB_Creator
{
    public class czTable_Cmd_Create_Base:czTable_Cmd_Create_Core
    {
        readonly ceTable_SQL_Type _Table_Type;

        //public czTable_Cmd_Create_Base()
        //{

        //}
        public czTable_Cmd_Create_Base(ceTable_SQL_Type cxTable_Type)
        {
            _Table_Type=cxTable_Type;
        }



        protected override string cfGet_Sql_type(Type cxType)
        {
            return _Table_Type switch
            {
                ceTable_SQL_Type.SQLite => cfGet_Sql_type_SQLite(cxType),
                ceTable_SQL_Type.MSQL => cfGet_Sql_type_MSQL(cxType),
                _ => throw new Exception("czTable_Cmd_Create_Base cfGet_Sql_type. There is no sush type of DB")
            };
        }



        protected override string cfGet_Value_Attribute(czaTableAttribute cxAttribute)
        {
            Type cxType = cxAttribute.GetType();
            if (cxType==typeof(czaPrimaryKeyAttribute)) return (((czaPrimaryKeyAttribute)cxAttribute).cfGet_Value(_Table_Type));
            else return base.cfGet_Value_Attribute(cxAttribute);
        }








        protected string cfGet_Sql_type_SQLite(Type cxType)
        {
            if (cxType.GetTypeInfo().IsEnum || cxType == typeof(int) || cxType == typeof(bool) || cxType == typeof(Boolean) || cxType == typeof(Byte) ||  cxType == typeof(SByte) || cxType == typeof(UInt16) || cxType == typeof(Int16) || cxType == typeof(Int32) || cxType == typeof(UInt32) || cxType == typeof(Int64) ||

                                               cxType == typeof(int?) || cxType == typeof(bool?) || cxType == typeof(Boolean?) || cxType == typeof(Byte?) ||  cxType == typeof(SByte?) || cxType == typeof(UInt16?) || cxType == typeof(Int16?) || cxType == typeof(Int32?) || cxType == typeof(UInt32?) || cxType == typeof(Int64?) ||
                                               cxType == typeof(TimeSpan) || cxType == typeof(DateTime) || cxType == typeof(DateTimeOffset))
            {
                return "INTEGER";
            }
            else if (cxType == typeof(Double) ||cxType == typeof(Single) || cxType == typeof(Double?) ||cxType == typeof(Single?))
            {
                return "REAL"; 
            }
            else if (cxType == typeof(Decimal)|| cxType == typeof(Decimal?))
            {
                return "NUMERIC";
            }
            else if (cxType == typeof(String) || cxType == typeof(Guid) || cxType == typeof(StringBuilder) || cxType == typeof(Uri) || cxType == typeof(UriBuilder))
            {
                return "TEXT";
            }
            else if (cxType == typeof(byte[]))
            {
                return "BLOB";
            }

            throw new Exception($"czTable_Cmd_Create_Core cfGet_Sql_type type {cxType.Name} not exist");
        }















        //protected string cfGet_Sql_Attribute_MSQL(czTableAttribute[] cxAttributes)
        //{
        //    if (cxAttributes?.Any()??false)
        //    {
        //        throw new NotImplementedException("cfGet_Fields_Params_of_Type");
        //    }
        //    else return null;
        //}

        protected string cfGet_Sql_type_MSQL(Type cxType)
        {
            if (cxType == typeof(Boolean) || cxType == typeof(bool) || cxType == typeof(Boolean?) || cxType == typeof(bool?))
            {
                return "bit";
            }
            if (cxType == typeof(Byte) ||  cxType == typeof(SByte) || cxType == typeof(Byte?) ||  cxType == typeof(SByte?) || cxType.GetTypeInfo().IsEnum)
            {
                return "tinyint";
            }
            if (cxType == typeof(UInt16) || cxType == typeof(Int16) || cxType == typeof(UInt16?) || cxType == typeof(Int16?))
            {
                return "smallint";
            }
            if (cxType == typeof(Int32) || cxType == typeof(UInt32) || cxType == typeof(Int32?) || cxType == typeof(UInt32?))
            {
                return "int";
            }
            if (cxType == typeof(Int64) || cxType == typeof(Int64?))
            {
                return "bigint";
            }
            else if (cxType == typeof(Double) ||cxType == typeof(Single) || cxType == typeof(Double?) ||cxType == typeof(Single?))
            {
                return "float";
            }
            else if (cxType == typeof(Decimal) || cxType == typeof(Decimal?))
            {
                return "numeric(10,5)";//max (18,18) default (18,0)
            }
            else if (cxType == typeof(Guid))
            {
                return "varchar(36)";
            }
            else if (cxType == typeof(String) || cxType == typeof(StringBuilder) || cxType == typeof(Uri) || cxType == typeof(UriBuilder))
            {
                return "varchar(MAX)";
            }



            if (cxType == typeof(TimeSpan))
            {
                return "timestamp";
            }
            else if (cxType == typeof(DateTime))
            {
                return "datetime";
            }
            else if (cxType == typeof(DateTimeOffset))
            {
                return "datetimeoffset(7)";
            }

            throw new Exception($"czTable_Cmd_Create_Core cfGet_Sql_type type {cxType.Name} not exist");
        }




    }
}
