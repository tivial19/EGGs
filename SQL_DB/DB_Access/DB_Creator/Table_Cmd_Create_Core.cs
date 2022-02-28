using SQL_DB.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQL_DB.DB_Access.DB_Creator
{
    public class czTable_Cmd_Create_Core
    {
        const string ccCreate_cmd_format = "CREATE TABLE {0} ({1});";

        public czTable_Cmd_Create_Core()
        {

        }

        public string cfGet_Create_Cmd<T>(string cxTable_Name = null)
        {
            return cfGet_Create_Cmd(typeof(T), cxTable_Name);
        }

        public string cfGet_Create_Cmd(Type cxType, string cxTable_Name = null)
        {
            var Q = cfGet_Table_Params_of_Type(cxType);
            if (Q.Fields_Attributes.Any())
            {
                string[] cxParams = cfGet_Table_Params(Q.Fields_Attributes, Q.Table_Attributes);
                return cfGet_Create_Cmd(cxType, cxTable_Name, cxParams);
            }
            else throw new Exception("cfGet_Fields_Params_of_Type of type " + cxType.Name + "error");
        }


        public string cfGet_Create_Cmd(string cxTable_Name, string[] cxParams)
        {
            string cxParam = string.Join(", ", cxParams);
            return string.Format(ccCreate_cmd_format, cxTable_Name, cxParam);
        }

        public string cfGet_Create_Cmd<T>(string[] cxParams, string cxTable_Name=null)
        {
            return cfGet_Create_Cmd(typeof(T), cxParams,cxTable_Name);
        }

        public string cfGet_Create_Cmd(Type cxType, string[] cxParams, string cxTable_Name=null)
        {
            return cfGet_Create_Cmd(cxType, cxTable_Name, cxParams);
        }



        protected string cfGet_Create_Cmd<T>(string cxTable_Name, string[] cxParams)
        {
            return cfGet_Create_Cmd(typeof(T), cxTable_Name, cxParams);
        }
        protected string cfGet_Create_Cmd(Type cxType, string cxTable_Name, string[] cxParams)
        {
            string cxName = string.IsNullOrWhiteSpace(cxTable_Name) ? cxType.Name : cxTable_Name;
            return cfGet_Create_Cmd(cxName, cxParams);
        }





        protected ((string Name, Type Type, czaTableAttribute[] Attrs)[] Fields_Attributes, czaTableAttribute[] Table_Attributes) cfGet_Table_Params_of_Type<T>()
        {
            return cfGet_Table_Params_of_Type(typeof(T));
        }

        protected ((string Name, Type Type, czaTableAttribute[] Attrs)[] Fields_Attributes, czaTableAttribute[] Table_Attributes) cfGet_Table_Params_of_Type(Type cxType)
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














        protected virtual string[] cfGet_Table_Params((string Name, Type Type, czaTableAttribute[] Attrs)[] cxFields_Attributes, czaTableAttribute[] cxTable_Attributes)
        {
            List<string> cxParams_fields = new List<string>();
            List<string> cxParams_table = new List<string>();

            foreach (var cxItem in cxFields_Attributes)
            {
                string cxField = cxItem.Name + " " + cfGet_Sql_type(cxItem.Type);//field name type
                var Qf = cfGet_Field_Attributes(cxItem.Type, cxItem.Attrs);
                if (!string.IsNullOrWhiteSpace(Qf.Field_add)) cxField+=" " + Qf.Field_add;// add field attribute
                if (Qf.Table_add?.Any()??false) cxParams_table.AddRange(Qf.Table_add);// add table attribute from field attribut
                cxParams_fields.Add(cxField);
            }
            
            var Qa = cfGet_Table_Attributes(cxTable_Attributes);
            if (Qa?.Any()??false) cxParams_table.AddRange(Qa);
            var Q = cxParams_table.Where(s => !string.IsNullOrWhiteSpace(s));
            if (Q.Any()) cxParams_fields.AddRange(Q);
            return cxParams_fields.ToArray();
        }



        protected virtual (string Field_add, List<string> Table_add) cfGet_Field_Attributes(Type cxField_Type, czaTableAttribute[] cxAttributes)
        {
            List<string> Field_add = new List<string>();
            List<string> cxParams_table = new List<string>();

            czaTableAttribute[] cxAttributes_Corr = cfGet_Attribute_Correct(cxField_Type, cxAttributes);

            foreach (var cxA in cxAttributes_Corr)//here you can:
            {
                if (cxA is czaCheckOneAttribute)
                {
                    cxParams_table.Add(cfGet_Value_Attribute(cxA));//add to table
                }
                else Field_add.Add(cfGet_Value_Attribute(cxA));//add to field
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


                //int cxANN = 0;
                //if (cxR.Count(a => a.GetType()==typeof(czaNullAttribute))>0) cxANN++;
                //if (cxR.Count(a => a.GetType()==typeof(czaNotNullAttribute))>0) cxANN++;

                //if (cfIs_Type_Nullable(cxType))
                //{
                //    if (cxANN==0) cxR.Add(new czaNullAttribute());
                //}
                //else
                //{
                //    if (cxANN==0) cxR.Add(new czaNotNullAttribute());
                //}
                return cxR.ToArray();



                static bool cfIs_NOT_Nullable(Type cxType)
                {
                    if (cxType==typeof(string)) return false;
                    //else (cxType==typeof(Nullable<string>)) return true;
                    else
                    {
                        //if(cxType.IsGenericType)
                        //{
                        //    var x = cxType.GetGenericTypeDefinition();

                        //}
                        return Nullable.GetUnderlyingType(cxType)==null;
                        //return false;
                        //return cxType.IsGenericType && cxType.GetGenericTypeDefinition()==typeof(Nullable<>);// Nullable.GetUnderlyingType(cxType)!=null;
                    }
                }
            }

        }

        protected virtual string[] cfGet_Table_Attributes(czaTableAttribute[] cxAttributes)
        {
            if (cxAttributes?.Any()??false) return cxAttributes.Where(a => a!=null).Select(a => cfGet_Value_Attribute(a)).ToArray();
            else return null;
            //if (cxType==typeof(czaUniqueAttribute)) return "CONSTRAINT FGFDG_ UNIQUE (FIELD1,    FIELD2)";
            //if (cxType==typeof(czaCheckAttribute)) return "CONSTRAINT FGFDG_ CHECK (value)";
        }




        protected virtual string cfGet_Value_Attribute(czaTableAttribute cxAttribute)
        {
            czaTable_ValueAttribute cxAv = cxAttribute as czaTable_ValueAttribute;
            if (cxAv!=null) return cxAv.Value;
            else
            {
                return null;
            }

            //if (cxAttribute is czaTable_ValuesAttribute) throw new Exception("cfGet_Value_Attribute has type czaTable_ValuesAttribute use cfGet_Values_Attribute");
            //if (cxAttribute is czaTable_ValueAttribute) return ((czaTable_ValueAttribute)cxAttribute).Value;
            //else return null;
        }





        protected virtual string cfGet_Sql_type(Type cxType)
        {
            throw new Exception("czTable_Cmd_Create_Core cfGet_Sql_type cannot get type use overrite function in czTable_Cmd_Create_Base");
        }


















        //private string cfGet_Type_Name<T>()
        //{
        //    return typeof(T).Name;
        //}




    }
}
