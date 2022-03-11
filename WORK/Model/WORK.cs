using cpADD;
using cpADD.Ability;
using cpWORK.Enties;
using cpWORK.Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cpWORK
{
    public class czWork : czAPP_model, czIWork
    {

        czDB _DB;
        czIRemoteFileServer _RFS;

        public czWork(/*czIJSON cxjson,*/ czIRemoteFileServer cxRFS, czIAPP cxApp, czITrace cxTrace, czISerializer cxSer) : base(cxApp, cxTrace)
        {
            _RFS = cxRFS;

            Filter = new czFilter();
            cfLoad();


            async void cfLoad()
            {
                await cfCreate_DB(cxSer);
                Status = "cfCreate_Tables";

                //await cfLoad_json();

                await cfFind();
                //await cfLoad_ALL();
            }

            Task cfCreate_DB(czISerializer cxSer)
            {
                _DB = new czDB(_APP, cxSer);
                return _DB.cfCreate_DB_and_AllTables();
            }

        }




        public List<czItem> View { get; private set; }
        public czItem Selected_Item { get; set; }
        public bool isSort_Reverse { get; set; }
        public czFilter Filter { get; set; }


        public string Status_View { get; set; }

        public bool isAdd_Auto_Symbol_to_End { get; set; } = true;
        public bool isAdd_Auto_Symbol_to_Start { get; set; } = true;
        public bool isUse_Where_directly { get; set; }



        public async Task cfLoad_ALL()
        {
            Filter = new czFilter();
            await cfLoad_all();
        }


        private async Task cfLoad_all()
        {
            var Qa = await _DB.cfLoad_ALL();
            await cfSet_View(Qa, true);
        }



        public async Task cfFind()
        {
            if (Filter.isEmpty) await cfLoad_all();
            else
            {
                var Qa = await _DB.cfLoad_Filter(Filter, isAdd_Auto_Symbol_to_Start, isAdd_Auto_Symbol_to_End, isUse_Where_directly);
                await cfSet_View(Qa,false);
            }
        }

        public async Task cfAdd(czItem cxRow)
        {
            await _DB.cfInsert_Row(cxRow);
            await cfLoad_ALL();
        }



        public async Task cfReplace(czItem cxRow)
        {
            await _DB.cfReplace_Row(cxRow);
            await cfLoad_ALL();
        }

        public async Task cfDelete(czItem cxRow)
        {
            if (cxRow != null)
            {
                if (true == await _APP.cfMSG_YesCancel($"Удалить {cxRow.Name_simple}"))
                {
                    await _DB.cfDelete_Row(cxRow);
                    await cfLoad_ALL();
                }
            }
            else _APP.cfMSG_Info("Элемент не выбран!",true);
        }






        async Task cfSet_View(IEnumerable<czItem> cxData, bool isAll)
        {
            if (cxData!=null)
            {
                View = new List<czItem>(cfSort(cxData));
                Status_View = cfGet_Status_View();
                if (isAll) Status = Status_View;
                else Status = await cfGet_Status_ALL();
            }
            else
            {
                View = new List<czItem>();
                Status = $"Список элементов пуст.";
            }

            IEnumerable<czItem> cfSort(IEnumerable<czItem> cxSource)
            {
                if(!isSort_Reverse) return cxSource.OrderByDescending(s => s.Date).AsEnumerable();
                else return cxSource.OrderBy(s => s.Date);
            }


            string cfGet_Status_View()
            {
                return "Итого" + cfGet_Status(cfCalc(View));
            }

            async Task<string> cfGet_Status_ALL()
            {
                var Qa = await _DB.cfLoad_ALL();
                return "Всего" + cfGet_Status(cfCalc(Qa));
            }



            string cfGet_Status((int cxSum1, int cxSum2, decimal cxSum_money, int cxDays) cxV)
            {
                return $" за {cxV.cxDays} дней: {cxV.cxSum1} и {cxV.cxSum2} шт. Доход={cxV.cxSum_money}р."; 
            }



            (int cxSum1, int cxSum2, decimal cxSum_money, int cxDays) cfCalc(IEnumerable<czItem> cxItems)
            {
                int cxSum1 = cxItems.Sum(s=>s.Count_1);
                int cxSum2 = cxItems.Sum(s => s.Count_2);
                int cxDays= cxItems.Count();
                decimal cxSum_money = cxItems.Sum(s => s.Money); 

                return (cxSum1, cxSum2, cxSum_money, cxDays);
            }


        }






        public Action<czItem> _View_Item_Show { get; set; }



        int _View_Item_Show_New_Edit_Copy;

        public  void cfView_Item_Show(int cxNew_Edit_Copy)
        {
            _View_Item_Show_New_Edit_Copy = cxNew_Edit_Copy;

            czItem cxTshow = null;
            switch (cxNew_Edit_Copy)
            {

                case 1: cxTshow = Selected_Item;
                    break;
                
                
                default: cxTshow = new czItem();
                    break;
            }


            if (cxTshow != null) _View_Item_Show(cxTshow);


            //czItem cfGet_Clone(czItem cxT)
            //{
            //    return new czItem() { id = cxT.id, Comment = cxT.Comment, Date = new DateTime(cxT.Date.Ticks), Count_1 = cxT.Count_1, Count_2 = cxT.Count_2 };
            //}
        }

 


        public async Task cfView_Item_OK(czItem cxTable)
        {
            switch (_View_Item_Show_New_Edit_Copy)
            {

                case 1:
                    await cfReplace(cxTable);
                    break;

                default:
                    await cfAdd(cxTable);
                    break;
            }
        }












        public async Task cfShow_Data_Fields()
        {
            await _APP.cfMSG_OK(_DB.cfGet_Tables_Colums_text(), "Поля данных:");
        }




        public async Task cfWrite_Data_Remote()
        {
            await cfLoad_ALL();
            if (View.Count>0)
            {
                if (true == await _APP.cfMSG_YesNo("Все данные будут сохранены на сервере. Продолжить?"))
                    await _RFS.cfWrite_DATA_FILE(_DB.File);
            }
            else throw new czAPP_Exception_Normal("Элементы не найдены!");
        }

        public async Task cfRead_Data_Remote()
        {
            if (true == await _APP.cfMSG_YesNo("Все данные будут загружены с сервера. Продолжить?"))
            {
                if (true == await _RFS.cfRead_Override_DATA_FILE(_DB.File))
                {
                    await cfLoad_ALL();
                }
            }
        }





        public async Task cfBackUp(bool cxView_Only)
        {
            string cxMsg, cxMsg_format = "{0} данные будут сохранены в json. Продолжить ?";
            czItem[] cxExport = null;
            if (cxView_Only)
            {
                if (View?.Any()??false)
                {
                    cxExport=View.ToArray();
                    cxMsg="Видимые";
                }
                else throw new czAPP_Exception_Normal("Список элементов пуст");
            }
            else cxMsg="Все";

            cxMsg=string.Format(cxMsg_format, cxMsg);
            if (true == await _APP.cfMSG_YesNo(cxMsg)) await _DB.cfBack_UP_json(cxExport);
        }


        public async Task cfRestore()
        {
            int? r = await _APP.cfMSG_Universal("Данные будут загружены с резерва json. Добавить или перезаписать?", "Добавить", "Перезаписать");
            if (r>0)
            {
                var Qa = await _DB.cfRestore_json(r==1);
                await cfSet_View(Qa, true);
            }
        }







    }
}
