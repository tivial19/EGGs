using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_DB.Example
{


    public class czClass : czTable_Sql_Base<czItem_Data>
    {
        public czClass(Func<IDbConnection> cxConn, string cxName = null) : base(cxConn, cxName)
        {
        }
    }


    public class czTable_Test : czClass//czTable_Sql_Base<czItem_Data>
    {
        public czTable_Test(Func<IDbConnection> cxConn, string cxName = null) : base(cxConn, cxName)
        {

        }

        public Task cfInsert_Random_Items(int cxCount)
        {
            Stopwatch cxSW;

            cxSW = Stopwatch.StartNew();



            //return cfInsert(cfGet_Items_Data(cxCount).ToArray());
            return cfInsert_Quick(cfGet_Items_Data(cxCount).ToArray());
            //return cfInsert_Manual(cfGet_Items_Data(cxCount).ToArray());



            //return cfUpdate(cfGet_Items_Data(cxCount).ToArray());
            //string cxS = cxSW.Elapsed.ToString();
        }




        //"insert into czItem_Data (\"Value\", \"Text\") values (@Value, @Text)"
        public Task cfInsert_Manual(params czItem_Data[] cxItems)
        {
            czItem_Data cxItem;
            string cxValues = cfGet_Values(cxItems);

            string cxInsert = $"INSERT INTO {nameof(czItem_Data)} ({nameof(cxItem.Value)},{nameof(cxItem.Text)}) VALUES {cxValues}";
            return cfCMD_Execute(cxInsert);

            static string cfGet_Values(IEnumerable<czItem_Data> cxItems)
            {
                var Q = cxItems.Select(s => cfGet_Values(s));
                string cxR = string.Join(",", Q);
                return cxR;

                static string cfGet_Values(czItem_Data cxItem)
                {
                    return $"({cxItem.Value},'{cxItem.Text}')";
                }
            }
        }









        public Task<czItem_Data[]> cfLoad_Items_Text_Content(string cxText_Filter)
        {
            czItem_Data cxI;
            string cxWhere = $"{nameof(cxI.Text)} Like '%{cxText_Filter}%'";
            return cfLoad_Where(cxWhere);
        }

        private IEnumerable<czItem_Data> cfGet_Items_Data(int cxCount)
        {
            for (int i = 0; i < cxCount; i++) yield return new czItem_Data(i+1, "Text " + (4 * i + 9).ToString());
        }






    }


}

