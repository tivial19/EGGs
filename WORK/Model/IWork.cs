using cpWORK.Enties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cpWORK
{
    public interface czIWork
    {
        string Status_View { get; }
        string Status { get; }

        bool isSort_Reverse { get; set; }

        czItem Selected_Item { get; set; }
        List<czItem> View { get; }
        czFilter Filter { get; set; }


        Task cfLoad_ALL();

        Task cfFind();

        Task cfDelete(czItem cxRow);

        Task cfWrite_Data_Remote();
        Task cfRead_Data_Remote();


        Action<czItem> _View_Item_Show { get; set; }
        void cfView_Item_Show(int cxNew_Edit_Copy);

        Task cfView_Item_OK(czItem cxTable);



        bool isAdd_Auto_Symbol_to_End { get; set; }
        bool isAdd_Auto_Symbol_to_Start { get; set; }
        bool isUse_Where_directly { get; set; }


        Task cfShow_Data_Fields();
        Task cfBackUp(bool cxView_Only);
        Task cfRestore();


    }
}