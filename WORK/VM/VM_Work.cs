using cpADD;
using cpADD.Ability;
using cpADD.APP;
using cpWORK.Enties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cpWORK
{
    public class czVM_Work:czVM, czIWork
    {

        czIWork _Work;

        public czVM_Work(czIWork cxWork, czIAPP cxApp, czITrace cxTrace) :base(cxApp,cxApp,cxTrace)
        {
            _Work = cxWork;
        }



        public string Status => _Work.Status;
        public string Status_View => _Work.Status_View;

        public List<czItem> View => _Work.View;

        public czItem Selected_Item { get => _Work.Selected_Item; set => _Work.Selected_Item = value; }

        public bool isSort_Reverse { get => _Work.isSort_Reverse; set => _Work.isSort_Reverse = value; }

        public czFilter Filter { get => _Work.Filter; set => _Work.Filter = value; }



        public czICMD_Base ccLoad_ALL { get => _ccLoad_ALL ?? (_ccLoad_ALL = new czCMD_Base(cfLoad_ALL)); }private czICMD_Base _ccLoad_ALL;

        public Task cfLoad_ALL()
        {
            return _Work.cfLoad_ALL();
        }



        public czICMD_Base ccFind { get => _ccFind ?? (_ccFind = new czCMD_Base(cfFind)); } private czICMD_Base _ccFind;

        public Task cfFind()
        {
            return _Work.cfFind();
        }



        //public czICMD_Base ccAdd { get => _ccAdd ?? (_ccAdd = new czCMD_Base(d=>cfAdd(d as czITable))); } private czICMD_Base _ccAdd;

        //public Task cfAdd(czITable cxRow)
        //{
        //    return _Work.cfAdd(cxRow);
        //}




        //public czICMD_Base ccReplace { get => _ccReplace ?? (_ccReplace = new czCMD_Base(d=>cfReplace(d as czITable))); } private czICMD_Base _ccReplace;

        //public Task cfReplace(czITable cxRow)
        //{
        //    return _Work.cfReplace(cxRow);
        //}




        public czICMD_Base ccDelete { get => _ccDelete ?? (_ccDelete = new czCMD_Base(d=>cfDelete(d as czItem))); } private czICMD_Base _ccDelete;

        public Task cfDelete(czItem cxRow)
        {
            return _Work.cfDelete(cxRow);
        }










        public czICMD_Base ccWrite_Data_Remote { get => _ccWrite_Data_Remote ?? (_ccWrite_Data_Remote = new czCMD_Base(cfWrite_Data_Remote)); }
        private czICMD_Base _ccWrite_Data_Remote;
        public Task cfWrite_Data_Remote()
        {
            return _Work.cfWrite_Data_Remote();
        }




        public czICMD_Base ccRead_Data_Remote { get => _ccRead_Data_Remote ?? (_ccRead_Data_Remote = new czCMD_Base(cfRead_Data_Remote)); }
        private czICMD_Base _ccRead_Data_Remote;
        public Task cfRead_Data_Remote()
        {
            return _Work.cfRead_Data_Remote();
        }








        public Action<czItem> _View_Item_Show { get => _Work._View_Item_Show; set => _Work._View_Item_Show = value; }



        public czICMD_Base ccView_Item_Show { get => _ccView_Item_Show ?? (_ccView_Item_Show = new czCMD_Base(d => cfView_Item_Show((d == null) ? 0 : Convert.ToInt32(d)))); }private czICMD_Base _ccView_Item_Show;
        public void cfView_Item_Show(int cxNew_Edit_Copy)
        {
            _Work.cfView_Item_Show(cxNew_Edit_Copy);
        }

        public czICMD_Base ccView_Item_OK { get => _ccView_Item_OK ?? (_ccView_Item_OK = new czCMD_Base(d=>cfView_Item_OK(d as czItem))); }

        

        private czICMD_Base _ccView_Item_OK;
        public Task cfView_Item_OK(czItem cxRow)
        {
            return _Work.cfView_Item_OK(cxRow);
        }










        public bool isAdd_Auto_Symbol_to_End { get => _Work.isAdd_Auto_Symbol_to_End; set => _Work.isAdd_Auto_Symbol_to_End=value; }
        public bool isAdd_Auto_Symbol_to_Start { get => _Work.isAdd_Auto_Symbol_to_Start; set => _Work.isAdd_Auto_Symbol_to_Start=value; }
        public bool isUse_Where_directly { get => _Work.isUse_Where_directly; set => _Work.isUse_Where_directly=value; }


        public czICMD_Base ccBackUp { get => _ccBackUp ?? (_ccBackUp=new czCMD_Base(d => cfBackUp(d==null ? false : Convert.ToBoolean(d)))); }
        private czICMD_Base _ccBackUp;

        public Task cfBackUp(bool cxView_Only)
        {
            return _Work.cfBackUp(cxView_Only);
        }

        public czICMD_Base ccRestore { get => _ccRestore ?? (_ccRestore=new czCMD_Base(cfRestore)); }
        private czICMD_Base _ccRestore;

        public Task cfRestore()
        {
            return _Work.cfRestore();
        }

        public czICMD_Base ccShow_Data_Fields { get => _ccShow_Data_Fields ?? (_ccShow_Data_Fields=new czCMD_Base(cfShow_Data_Fields)); }
        private czICMD_Base _ccShow_Data_Fields;

        public Task cfShow_Data_Fields()
        {
            return _Work.cfShow_Data_Fields();
        }






    }


}
