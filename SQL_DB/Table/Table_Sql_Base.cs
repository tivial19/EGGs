using Dapper;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SQL_DB.Data;
using SQL_DB.Table;

namespace SQL_DB
{
    public class czTable_Sql_Base<T> : czTable_Sql_Core, czITable_Sql_Base//, czITable_Data //where T: new() //need for deleteAll
    {
        public Type Data_Type => typeof(T);


        public czTable_Sql_Base(Func<IDbConnection> cxConn, string cxName = null) : base(cxConn, cxName==null ? typeof(T).Name : cxName)
        {

        }


        public string[] cfGet_Data_Columns()
        {
            var cxProps = cfGet_Properties_ALL_Set_noIgnore(typeof(T), BindingFlags.Public |  BindingFlags.Instance);
            return cxProps.Select(p => p.Name).ToArray();
        }


        public async Task<int> cfCount()
        {
            int cxC = await _conn().QuerySingleAsync<int>($"Select Count(*) From {Table_Name}");
            return cxC;
        }


        public async Task<T[]> cfLoad_cmd(string cxCmd)
        {
            var Qr = await _conn().QueryAsync<T>(cxCmd);
            return Qr.ToArray();
        }

        public Task<T[]> cfLoad_All()
        {
            return cfLoad_cmd(cfGet_cmd_Select());
        }

        public Task<T[]> cfLoad_Where(string cxWhere)
        {
            return cfLoad_cmd(cfGet_cmd_Select(cxWhere));
        }

        public Task<T[]> cfLoad_Where_End(string cxWhere, string cxEnd)
        {
            return cfLoad_cmd(cfGet_cmd_Select(cxWhere,cxEnd));
        }

        public Task<T[]> cfLoad_Select_Where_End(string cxSelect, string cxWhere, string cxEnd)
        {
            return cfLoad_cmd(cfGet_cmd_Select(cxSelect, cxWhere, cxEnd));
        }



        public async Task<F[]> cfLoad_Field<F>(string cxField)
        {
            var Qr = await _conn().QueryAsync<F>($"Select DISTINCT \"{cxField}\" From {Table_Name}");
            return Qr.ToArray();
        }





        public Task<int> cfInsert(T cxItems, bool? cxPrimaryKey_Include = null)
        {
            return cfInsert(new T[] { cxItems }, cxPrimaryKey_Include);
        }

        public Task<int> cfInsert(T[] cxItems, bool? cxPrimaryKey_Include = null)
        {
            string[] cxPrs = cfGet_Properties_Names(typeof(T), cxPrimaryKey_Include);
            string cxColumns_Names = cfGet_Columns_Names(cxPrs);
            string cxParametrs = cfGet_Parameters_Names(cxPrs);
            string cxInsert_cmd = $"INSERT INTO {Table_Name} ({cxColumns_Names}) VALUES ({cxParametrs})";

            return cfCMD_Execute(cxInsert_cmd, cxItems);
        }

        public Task<int> cfInsert_Quick(T[] cxItems, bool? cxPrimaryKey_Include = null)
        {
            var cxPI = cfGet_Properties_type(typeof(T), cxPrimaryKey_Include);
            string cxColumns_Names = cfGet_Columns_Names(cxPI);
            string cxValues = cfGet_Columns_Value(cxPI, cxItems);
            string cxInsert_cmd = $"INSERT INTO {Table_Name} ({cxColumns_Names}) VALUES {cxValues}";

            return cfCMD_Execute(cxInsert_cmd);
        }






        public Task<int> cfUpdate(params T[] cxItems)//Update only one Item
        {
            //update czItem_Data set "Value" = @Value, "Text" = @Text where "id" = @id
            if (cxItems==null || !cxItems.Any()) throw new Exception("cfUpdate cannot Update empty");

            string cxId_Field = cfGet_Primary_Key();
            string cxFormat = "\"{0}\" = @{0}";// where id=5
            string cxWhere = string.Format(cxFormat, cxId_Field);

            string[] cxPrs = cfGet_Properties_Names(typeof(T), false);
            string cxParameters = string.Join(", ", cxPrs.Select(s => string.Format(cxFormat, s)));
            return cfUpdate(cxParameters, cxWhere, cxItems);
        }

        public Task<int> cfUpdate(string cxParameters, string cxWhere, object cxParam = null)
        {
            if (string.IsNullOrWhiteSpace(cxParameters) || string.IsNullOrWhiteSpace(cxWhere)) throw new Exception("cfUpdate cannot Update empty");
            return Task.Run<int>(() =>
            {
                string cxCMD = $"UPDATE {Table_Name} SET {cxParameters} Where {cxWhere}";
                return cfCMD_Execute(cxCMD, cxParam);
            });
        }




        public Task<int> cfDelete(params T[] cxItems)
        {
            //delete from czItem_Data where "id" = @id
            if (cxItems==null || !cxItems.Any()) throw new Exception("cfDelete cannot delete empty");

            string cxId_Field = cfGet_Primary_Key();
            string cxFormat = "\"{0}\" = @{0}";
            return cfDelete(string.Format(cxFormat, cxId_Field), cxItems);
        }

        public Task<int> cfDelete(string cxWhere, object cxParam = null)
        {
            if (string.IsNullOrWhiteSpace(cxWhere)) throw new Exception("cfDelete cannot delete empty");
            return Task.Run<int>(() =>
            {
                string cxCMD = $"DELETE FROM {Table_Name} Where {cxWhere}";
                return cfCMD_Execute(cxCMD, cxParam);
            });
        }




        public Task<int> cfClear()//delete with forgein
        {
            return Task.Run<int>(() =>
            {
                string cxCMD = $"DELETE FROM {Table_Name}";
                return cfCMD_Execute(cxCMD);
            });
        }

        public Task<int> cfClear_trunc()//not delete forgein 
        {
            //have no in sqllite
            //if (_Table_SQL_Type==ceTable_SQL_Type.SQLite) throw new Exception("SQLite have no TRUNCATE cmd. Use cfClear");
            return Task.Run<int>(() =>
            {
                string cxCMD = $"TRUNCATE TABLE {Table_Name}";
                return cfCMD_Execute(cxCMD);
            });
        }


        public Task<int> cfDrop()
        {
            return Task.Run<int>(() =>
            {
                string cxCMD = $"DROP TABLE {Table_Name}";
                return cfCMD_Execute(cxCMD);
            });
        }








        protected string cfGet_cmd_Select(string cxSelect, string cxWhere, string cxEnd)
        {
            string cxS = string.IsNullOrWhiteSpace(cxSelect) ? "*" : cxSelect;
            string cxCmd = $"Select {cxS} From {Table_Name}";
            if (!string.IsNullOrWhiteSpace(cxWhere)) cxCmd+=$" Where {cxWhere}";
            if (!string.IsNullOrWhiteSpace(cxEnd)) cxCmd+=" " + cxEnd;
            return cxCmd;
        }

        protected string cfGet_cmd_Select(string cxWhere, string cxEnd)
        {
            return cfGet_cmd_Select(string.Empty, cxWhere, cxEnd);
        }

        protected string cfGet_cmd_Select(string cxWhere)
        {
            return cfGet_cmd_Select(string.Empty, cxWhere, string.Empty);
        }

        protected string cfGet_cmd_Select()
        {
            return cfGet_cmd_Select(string.Empty, string.Empty, string.Empty);
        }














        protected string cfGet_Primary_Key()
        {
            var Qp = cfGet_Property_Attribute<czaPrimaryKeyAttribute>();
            if(Qp?.Any()??false)
            {
                if(Qp.Length>1) throw new Exception("cfGet_Primary_Key: " + typeof(T).Name + " have more than one czaPrimaryKeyAttribute");
                else return Qp.Single().Name;
            }
            else throw new Exception("cfGet_Primary_Key: " + typeof(T).Name + " have no czaPrimaryKeyAttribute");
        }

        protected (string Foreign_Key, string Foreign_Id) cfGet_Foreign_Key_Id(string cxTable_Main_Name)
        {
            var Qp = cfGet_Property_Attribute<czaForeignKeyAttribute>();
            if (Qp?.Any()??false)
            {
                var Q = Qp.Where(p => p.GetCustomAttribute<czaForeignKeyAttribute>().Table_Name==cxTable_Main_Name).Select(p=>(p.Name,p.GetCustomAttribute<czaForeignKeyAttribute>().Id_column_Name)).ToArray();
                if (Q.Length>1) throw new Exception("cfGet_Foreign_Key: " + typeof(T).Name + "have more than one czaForeignKeyAttribute with table name " + cxTable_Main_Name);
                return Q.Single();
            }
            else throw new Exception("cfGet_Foreign_Key: " + typeof(T).Name + " have no czaForeignKeyAttribute");
        }

        protected PropertyInfo[] cfGet_Property_Attribute<A>() where A : Attribute
        {
            var cxProps = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<A>()!=null);
            if (cxProps.Any()) return cxProps.ToArray();
            else return null;
        }










        private static string cfGet_Columns_Value(PropertyInfo[] cxPI, T[] cxItems)
        {
            string[] cxValues_AS = new string[cxItems.Length];
            for (int i = 0; i < cxItems.Length; i++) cxValues_AS[i] = cfGet_Values(cxItems[i], cxPI);
            return string.Join(", ", cxValues_AS);


            static string cfGet_Values(T cxItem, PropertyInfo[] cxValues)
            {
                var Q = cxValues.Select(p => cfGet_Value_Property(cxItem, p));
                return "(" + string.Join(",", Q) + ")";
            }

            static string cfGet_Value_Property(T cxItem, PropertyInfo cxProp)
            {
                string cxR = null;
                object cxO = cxProp.GetValue(cxItem);
                if (cxO!=null)
                {
                    if (cxProp.PropertyType.IsEnum) cxR=Convert.ToInt32(cxO).ToString();
                    else
                    {
                        cxR = cxO.ToString();
                        if (cxProp.PropertyType==typeof(string)) cxR=$"'{cxR}'";
                        else
                        {
                            if (decimal.TryParse(cxR, out decimal d))
                            {
                                cxR = cxR.Replace(",", ".");
                            }
                        }
                    }
                    return cxR;
                }
                else return "NULL";
            }
        }

        private static string cfGet_Columns_Names(PropertyInfo[] cxPI)
        {
            string[] cxPrs = cfGet_Properties_Names(cxPI);
            return cfGet_Columns_Names(cxPrs);
        }


        private static string[] cfGet_Properties_Names(PropertyInfo[] cxPI)
        {
            return cxPI.Select(p => p.Name).ToArray();
        }






        private static string cfGet_Columns_Names(string[] cxPrs)
        {
            string cxColumns_format = "\"{0}\"";//sqlite working for ms
            //if (_Table_SQL_Type==ceTable_SQL_Type.MSQL) cxColumns_format="[{0}]";
            return string.Join(", ", cxPrs.Select(s => string.Format(cxColumns_format, s)));
        }

        private static string cfGet_Parameters_Names(string[] cxPrs)
        {
            string cxParametrs_format = "@{0}";
            return string.Join(", ", cxPrs.Select(s => string.Format(cxParametrs_format, s)));
        }








        private static string[] cfGet_Properties_Names(Type cxType, bool? cxPrimaryKey_Include)
        {
            return cfGet_Properties_type(cxType, cxPrimaryKey_Include).Select(p => p.Name).ToArray();
        }


        private static PropertyInfo[] cfGet_Properties_type(Type cxType, bool? cxPrimaryKey_Include)
        {
            BindingFlags cxBindingAttr = BindingFlags.Public | BindingFlags.Instance;
            var Qa = cfGet_Properties_ALL_Set_noIgnore(cxType, cxBindingAttr);

            if (cxPrimaryKey_Include!=true)
            {
                var cxPrimary_Keys_Properties = cfGet_Properties_With_Attribute<czaPrimaryKeyAttribute>(cxType, cxBindingAttr);
                if (cxPrimary_Keys_Properties.Any())
                {
                    var cxPrimary_Keys_Attributes = cxPrimary_Keys_Properties.Select(p => p.GetCustomAttribute<czaPrimaryKeyAttribute>());
                    if (cxPrimary_Keys_Attributes.Any())
                    {
                        if (cxPrimaryKey_Include==false || (cxPrimaryKey_Include==null && cxPrimary_Keys_Attributes.First().AutoIncrement))
                        {
                            Qa=Qa.Except(cxPrimary_Keys_Properties);
                        }
                    }
                }
            }

            return Qa.ToArray();
        }




        private static IEnumerable<PropertyInfo> cfGet_Properties_ALL_Set_noIgnore(Type cxType, BindingFlags cxBindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            var Qa = cfGet_Properties_ALL_Set(cxType, cxBindingAttr);
            var Qi = cfGet_Properties_With_Attribute<czaIgnoreAttribute>(cxType, cxBindingAttr);
            if (Qi.Any()) return Qa.Except(Qi);
            else return Qa;
        }

        private static IEnumerable<PropertyInfo> cfGet_Properties_ALL_Set(Type cxType, BindingFlags cxBindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            return cxType.GetProperties(cxBindingAttr).Where(p => p.SetMethod!= null);
        }

        private static IEnumerable<PropertyInfo> cfGet_Properties_With_Attribute<A>(Type cxType, BindingFlags cxBindingAttr = BindingFlags.Public | BindingFlags.Instance) where A : Attribute
        {
            //GetProperties() = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
            return cxType.GetProperties(cxBindingAttr).Where(p => p.GetCustomAttribute<A>()!=null);
        }







        //private static PropertyInfo[] cfGet_Properties_type(Type cxType)
        //{
        //    Type cxIgnore_Attribute = typeof(czaIgnoreAttribute);
        //    Type cxPrimary_Key_Attribute = typeof(czaPrimaryKeyAttribute);

        //    var Qa = cxType.GetProperties().Where(p => p.SetMethod!= null);
        //    var Qi = Qa.Where(p => p.GetCustomAttributes(true).Any(a => a.GetType() == cxIgnore_Attribute));
        //    Qa = Qa.Except(Qi);

        //    var cxPrimary_Keys_Properties = Qa.Where(p => p.GetCustomAttributes(true).Any(a => a.GetType() == cxPrimary_Key_Attribute));
        //    if (cxPrimary_Keys_Properties.Any())
        //    {
        //        var cxPrimary_Keys_Attributes = cxPrimary_Keys_Properties.Select(p => p.GetCustomAttribute(cxPrimary_Key_Attribute));
        //        if (cxPrimary_Keys_Attributes.Any())
        //        {
        //            if (((czaPrimaryKeyAttribute)cxPrimary_Keys_Attributes.First()).AutoIncrement)
        //            {
        //                Qa=Qa.Except(cxPrimary_Keys_Properties);
        //            }
        //        }
        //    }

        //    return Qa.ToArray();
        //}


        //private static string[] cfGet_Properties_Names_Qua<P, I>(Type cxType) where P : Attribute where I : Attribute
        //{
        //    return cfGet_Properties_type<P, I>(cxType).Select(p => cfGet_Property_Name_Qua(p)).ToArray();

        //    static string cfGet_Property_Name_Qua(PropertyInfo cxProp)
        //    {
        //        string cxR = cxProp.Name;
        //        if (cxProp.PropertyType==typeof(string)) cxR=$"'{cxR}'";
        //        return cxR;
        //    }
        //}


    }
}
