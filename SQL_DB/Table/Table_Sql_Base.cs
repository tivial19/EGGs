using Dapper;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SQL_DB.Data;


namespace SQL_DB
{
    public class czTable_Sql_Base<T> : czTable_Sql_Core where T : class//, czITable_Data //where T: new() //need for deleteAll
    {
        public Type Data_Type => typeof(T);


        public czTable_Sql_Base(Func<IDbConnection> cxConn, string cxName=null) : base(cxConn,cxName==null ? typeof(T).Name:cxName)
        {

        }




        public async Task<int> cfCount()
        {
            int cxC = await _conn().QuerySingleAsync<int>($"Select Count(*) From {Table_Name}");
            return cxC;
        }

        public async Task<T[]> cfLoad_All()
        {
            var Qr = await _conn().QueryAsync<T>($"Select * From {Table_Name}");
            return Qr.ToArray();
        }


        public async Task<T[]> cfLoad_Where(string cxWhere)
        {
            var Qr = await _conn().QueryAsync<T>($"Select * From {Table_Name} Where {cxWhere}");
            return Qr.ToArray();
        }

        public async Task<F[]> cfLoad_Field<F>(string cxField)
        {
            var Qr = await _conn().QueryAsync<F>($"Select DISTINCT \"{cxField}\" From {Table_Name}");
            return Qr.ToArray();
        }








        public Task<int> cfInsert(params T[] cxItems)
        {
            string[] cxPrs = cfGet_Properties_Names(typeof(T));
            string cxColumns_Names = cfGet_Columns_Names(cxPrs);
            string cxParametrs = cfGet_Parameters_Names(cxPrs);
            string cxInsert_cmd = $"INSERT INTO {Table_Name} ({cxColumns_Names}) VALUES ({cxParametrs})";

            return cfCMD_Execute(cxInsert_cmd, cxItems);
        }

        public Task<int> cfInsert_Quick(params T[] cxItems)
        {
            var cxPI = cfGet_Properties_type(typeof(T));
            string cxColumns_Names = cfGet_Columns_Names(cxPI);
            string cxValues = cfGet_Columns_Value(cxPI,cxItems);
            string cxInsert_cmd = $"INSERT INTO {Table_Name} ({cxColumns_Names}) VALUES {cxValues}";

            return cfCMD_Execute(cxInsert_cmd);
        }






        public Task<int> cfUpdate(params T[] cxItems)
        {
            //update czItem_Data set "Value" = @Value, "Text" = @Text where "id" = @id
            if (cxItems==null || !cxItems.Any()) throw new Exception("cfUpdate cannot Update empty");

            string cxId_Field = cfGet_Primary_Key();
            string cxFormat = "\"{0}\" = @{0}";
            return cfUpdate(string.Format(cxFormat, cxId_Field), cxItems);
        }

        public Task<int> cfUpdate(string cxWhere, object cxParam = null)
        {
            if (string.IsNullOrWhiteSpace(cxWhere)) throw new Exception("cfUpdate cannot Update empty");
            return Task.Run<int>(() =>
            {
                string[] cxPrs = cfGet_Properties_Names(typeof(T));
                string cxFormat = "\"{0}\" = @{0}";
                string cxParameters = string.Join(", ", cxPrs.Select(s=>string.Format(cxFormat,s)));

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
            return cfDelete(string.Format(cxFormat, cxId_Field),cxItems);
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








        public string cfGet_Primary_Key()
        {
            return cfGet_Field_by_Attribute<czaPrimaryKeyAttribute>(string.Empty);
        }

        protected string cfGet_Foreign_Key(string cxTable_Main_Name)
        {
            return cfGet_Field_by_Attribute<czaForeignKeyAttribute>(cxTable_Main_Name);
        }

        protected string cfGet_Field_by_Attribute<A>(string cxName_Filter) where A : Attribute
        {
            var cxProps = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<A>()!=null);
            if (cxProps.Any())
            {
                if (!string.IsNullOrWhiteSpace(cxName_Filter)) cxProps=cxProps.Where(p => p.Name.StartsWith(cxName_Filter));
                if (cxProps.Any()) return cxProps.First().Name;
            }
            throw new Exception(typeof(T).Name + " have no " + typeof(A).Name + " " + cxName_Filter);
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
                    cxR = cxO.ToString();
                    if (cxProp.PropertyType==typeof(string)) cxR=$"'{cxR}'";
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








        private static string[] cfGet_Properties_Names(Type cxType) 
        {
            return cfGet_Properties_type(cxType).Select(p => p.Name).ToArray();
        }

        private static PropertyInfo[] cfGet_Properties_type(Type cxType) 
        {
            Type cxIgnore_Attribute = typeof(czaIgnoreAttribute);
            Type cxPrimary_Key_Attribute = typeof(czaPrimaryKeyAttribute);

            var Qa = cxType.GetProperties().Where(p=> p.SetMethod!= null);
            var Qi = Qa.Where(p => p.GetCustomAttributes(true).Any(a => a.GetType() == cxIgnore_Attribute));
            Qa = Qa.Except(Qi);

            var cxPrimary_Keys_Properties = Qa.Where(p => p.GetCustomAttributes(true).Any(a => a.GetType() == cxPrimary_Key_Attribute));
            if(cxPrimary_Keys_Properties.Any())
            {
                var cxPrimary_Keys_Attributes = cxPrimary_Keys_Properties.Select(p => p.GetCustomAttribute(cxPrimary_Key_Attribute));
                if(cxPrimary_Keys_Attributes.Any())
                {
                    if( ((czaPrimaryKeyAttribute)cxPrimary_Keys_Attributes.First()).AutoIncrement)
                    {
                        Qa=Qa.Except(cxPrimary_Keys_Properties);
                    }
                }
            }

            return Qa.ToArray();
        }






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
