using cpADD;
using System;
using System.Collections.Generic;
using System.Text;

namespace cpWORK.Enties
{
    public class czItem : czNotify_Object//, czITable_Data
    {
        //czITable_Data _data;

        public czTable_Data _data { get; }


        public czItem(czTable_Data cxData)
        {
            _data=cxData;
        }

        public czItem() : this(new czTable_Data())
        {

        }



        public long id { get => _data.id; set => _data.id=value; }
        public DateTime Date { get => new DateTime(_data.Date); set => _data.Date=value.Ticks; }
        public int Count_1 { get => _data.Count_1; set => _data.Count_1=value; }
        public int Count_2 { get => _data.Count_2; set => _data.Count_2=value; }
        public decimal Money { get => _data.Money; set => _data.Money=value; }
        public string Comment { get => _data.Comment; set => _data.Comment=value; }






        public string id_Date => cfGet_Id_Date();
        public string Name_simple => cfGet_Name();




        private string cfGet_Id_Date()
        {
            return new DateTime(id).ToString("dd.MM.yy HH:mm");
        }


        string cfGet_Name()
        {
            return $"{cfGet_Date()}";
        }

        string cfGet_Date()
        {
            return Date.ToString("dd.MM.yy");
        }



    }



}
