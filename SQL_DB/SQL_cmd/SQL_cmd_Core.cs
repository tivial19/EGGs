using System;
using System.Collections.Generic;
using SQL_DB.Data;
using System.Linq;
using System.Reflection;
using System.Text;


namespace SQL_DB.SQL_cmd
{


    public class czSQL_cmd_Core 
    {
        const string ccCreate_cmd_format = "CREATE TABLE {0} ({1});";

        public czSQL_cmd_Core()
        {

        }


        public string cfGet_Create_Cmd(string cxTable_Name, string[] cxParams)
        {
            string cxParam = string.Join(", ", cxParams);
            return string.Format(ccCreate_cmd_format, cxTable_Name, cxParam);
        }

        public string cfGet_Create_Cmd(ceTable_SQL_Type cxTable_Type, Type cxType, string cxTable_Name = null)
        {
            var Q = cfGet_Table_Params_of_Type(cxType);
            if (Q.Fields_Attributes.Any())
            {
                string[] cxParams = cfGet_Table_Params(cxTable_Type, Q.Fields_Attributes, Q.Table_Attributes);
                return cfGet_Create_Cmd(cfGet_Table_Name(cxType, cxTable_Name), cxParams);
            }
            else throw new Exception("cfGet_Fields_Params_of_Type of type " + cxType.Name + "error");
        }

        public string cfGet_Create_Cmd<T>(ceTable_SQL_Type cxTable_Type, string cxTable_Name = null)
        {
            return cfGet_Create_Cmd(cxTable_Type, typeof(T), cxTable_Name);
        }





        private string cfGet_Table_Name(Type cxType, string cxTable_Name)
        {
            if (string.IsNullOrWhiteSpace(cxTable_Name)) return cxType.Name;
            else return cxTable_Name;
        }





        private ((string Name, Type Type, czaTableAttribute[] Attrs)[] Fields_Attributes, czaTableAttribute[] Table_Attributes) cfGet_Table_Params_of_Type<T>()
        {
            return cfGet_Table_Params_of_Type(typeof(T));
        }

        private ((string Name, Type Type, czaTableAttribute[] Attrs)[] Fields_Attributes, czaTableAttribute[] Table_Attributes) cfGet_Table_Params_of_Type(Type cxType)
        {
            var cxProps = cxType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<czaIgnoreAttribute>()==null).ToArray();
            if (cxProps.Any())
            {
                List<(string Name, Type Type, czaTableAttribute[] Attrs)> cxFields_types = new List<(string Name, Type Type, czaTableAttribute[] Attrs)>();// Dictionary<string, Type>();

                for (int i = 0; i < cxProps.Length; i++)
                    cxFields_types.Add((cxProps[i].Name, cxProps[i].PropertyType, cxProps[i].GetCustomAttributes<czaTableAttribute>().ToArray()));

                var cxAttributes_table = cxType.GetCustomAttributes<czaTableAttribute>().ToArray();
                return (cxFields_types.ToArray(), cxAttributes_table);
            }
            else throw new Exception($"czTable_Cmd_Create_Core cfGet_Fields_Params_of_Type cxProps of type {cxType.Name} is empty");
        }



























        //ceTable_SQL_Type////////////////////////////////////////////////////////////////////




        private string[] cfGet_Table_Params(ceTable_SQL_Type cxTable_Type, (string Name, Type Type, czaTableAttribute[] Attrs)[] cxFields_Attributes, czaTableAttribute[] cxTable_Attributes)
        {
            List<string> cxParams_fields = new List<string>();
            List<string> cxParams_table = new List<string>();

            foreach (var cxItem in cxFields_Attributes)
            {
                string cxField = cxItem.Name + " " + cfGet_Sql_type(cxTable_Type, cxItem.Type);//field name type
                var Qf = cfGet_Field_Attributes(cxTable_Type, cxItem.Type, cxItem.Attrs);
                if (!string.IsNullOrWhiteSpace(Qf.Field_add)) cxField+=" " + Qf.Field_add;// add field attribute
                if (Qf.Table_add?.Any()??false) cxParams_table.AddRange(Qf.Table_add);// add table attribute from field attribut
                cxParams_fields.Add(cxField);
            }

            var Qa = cfGet_Table_Attributes(cxTable_Type, cxTable_Attributes);
            if (Qa?.Any()??false) cxParams_table.AddRange(Qa);
            var Q = cxParams_table.Where(s => !string.IsNullOrWhiteSpace(s));
            if (Q.Any()) cxParams_fields.AddRange(Q);
            return cxParams_fields.ToArray();
        }



        private (string Field_add, List<string> Table_add) cfGet_Field_Attributes(ceTable_SQL_Type cxTable_Type, Type cxField_Type, czaTableAttribute[] cxAttributes)
        {
            List<string> Field_add = new List<string>();
            List<string> cxParams_table = new List<string>();

            czaTableAttribute[] cxAttributes_Corr = cfGet_Attribute_Correct(cxField_Type, cxAttributes);

            foreach (var cxA in cxAttributes_Corr)//here you can:
            {
                if (cxA is czaCheckOneAttribute)
                {
                    cxParams_table.Add(cfGet_Value_Attribute(cxTable_Type, cxA));//add to table
                }
                else Field_add.Add(cfGet_Value_Attribute(cxTable_Type, cxA));//add to field
            }
            return (string.Join(" ", Field_add.Where(s => !string.IsNullOrWhiteSpace(s))), cxParams_table);



            static czaTableAttribute[] cfGet_Attribute_Correct(Type cxType, czaTableAttribute[] cxA)
            {
                if (cxA.Count(a => a.GetType()==typeof(czaPrimaryKeyAttribute))>0) return cxA;//Not null in sqlite make id not uniq
                List<czaTableAttribute> cxR = new List<czaTableAttribute>(cxA);

                bool isNull_Attr = cxR.Count(a => a.GetType()==typeof(czaNullAttribute))>0 || cxR.Count(a => a.GetType()==typeof(czaNotNullAttribute))>0;

                if (!isNull_Attr && cfIs_NOT_Nullable(cxType)) cxR.Add(new czaNotNullAttribute());

                if (!isNull_Attr)
                {
                    var x = cfIs_NOT_Nullable(cxType);
                }

                return cxR.ToArray();



                static bool cfIs_NOT_Nullable(Type cxType)
                {
                    if (cxType==typeof(string)) return false;
                    else return Nullable.GetUnderlyingType(cxType)==null;
                }
            }

        }

        private string[] cfGet_Table_Attributes(ceTable_SQL_Type cxTable_Type, czaTableAttribute[] cxAttributes)
        {
            if (cxAttributes?.Any()??false) return cxAttributes.Where(a => a!=null).Select(a => cfGet_Value_Attribute(cxTable_Type, a)).ToArray();
            else return null;
            //if (cxType==typeof(czaUniqueAttribute)) return "CONSTRAINT FGFDG_ UNIQUE (FIELD1,    FIELD2)";
            //if (cxType==typeof(czaCheckAttribute)) return "CONSTRAINT FGFDG_ CHECK (value)";
        }

        private string cfGet_Value_Attribute(ceTable_SQL_Type cxTable_Type, czaTableAttribute cxAttribute)
        {
            Type cxType = cxAttribute.GetType();
            if (cxType==typeof(czaPrimaryKeyAttribute)) return (((czaPrimaryKeyAttribute)cxAttribute).cfGet_Value(cxTable_Type));
            else
            {
                czaTable_ValueAttribute cxAv = cxAttribute as czaTable_ValueAttribute;
                if (cxAv!=null) return cxAv.Value;
                else return null;
            }
        }

        private string cfGet_Sql_type(ceTable_SQL_Type cxTable_Type, Type cxType)
        {
            return cxTable_Type switch
            {
                ceTable_SQL_Type.SQLite => cfGet_Sql_type_SQLite(cxType),
                ceTable_SQL_Type.MSQL => cfGet_Sql_type_MSQL(cxType),
                _ => throw new Exception("czTable_Cmd_Create_Base cfGet_Sql_type. There is no sush type of DB")
            };
        }






        private string cfGet_Sql_type_SQLite(Type cxType)
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
                return "REAL"; //return "NUMERIC"; can be like int64
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



        private string cfGet_Sql_type_MSQL(Type cxType)
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
                return "float"; //return "real"; 
            }
            else if (cxType == typeof(Decimal) || cxType == typeof(Decimal?))
            {
                return "decimal(10,3)"; //return "numeric(10,5)";//max (18,18) default (18,0) 
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
