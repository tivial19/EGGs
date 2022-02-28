using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.APP
{
    public class czAPP_test : czAPP_base, czIAPP
    {
        public czAPP_test(string cxPath) : base(cxPath)
        {
        }

        public czAPP_test(string cxPath, string cxName, string cxVersion) : base(cxPath, cxName, cxVersion)
        {
        }

        public czAPP_test(string cxPath_App, string cxName, string cxVersion, string cxPackage_Name, string cxDevice_Name) : base(cxPath_App, cxName, cxVersion, cxPackage_Name, cxDevice_Name)
        {
        }

        public czAPP_test(czAPP_Info_Create_Struct cxCreate_Info) : base(cxCreate_Info)
        {
        }




        public Task<DateTime?> cfDateDialog(DateTime cxDateStart, DateTime? cxbtn2 = null, DateTime? cxbtn3 = null, string cxTitle = null, string cxText = null)
        {
            throw new NotImplementedException();
        }

        public void cfMSG_Info(string cxText, bool cxLength_Long)
        {
            Debug.WriteLine(cxText);
        }

        public Task<string> cfMSG_Input(string cxText, string cxTitle = "Введите значение:", string cxPlaceHolder = null, string cxOK = "OK", string cxCancel = "Отмена", bool cxKeyBoardText = false, string cxInitialValue = "")
        {
            throw new NotImplementedException();
        }

        public Task<bool?> cfMSG_OK(string cxText, string cxTitle = "Внимание")
        {
            throw new NotImplementedException();
        }

        public Task<string> cfMSG_Select(string[] cxToSelect, string cxTitle = "Сделай свой выбор:", string cxCancel = "Отмена", string cxClear = "")
        {
            throw new NotImplementedException();
        }

        public Task<string> cfMSG_Select_File(string cxFile_Filter = null, string cxDir = null)
        {
            throw new NotImplementedException();
        }

        public Task<(bool isSelected, T Value)> cfMSG_Select_Object<T>(T[] cxObjects, Func<T, string> cxGet_Text, string cxTitle = "Сделай свой выбор:", string cxCancel = "Отмена", string cxClear = "")
        {
            throw new NotImplementedException();
        }

        public Task<int?> cfMSG_Universal(string cxText, string cxButton1 = "Да", string cxButton2 = "Нет", string cxButton0 = "Отмена", string cxTitle = "Внимание")
        {
            throw new NotImplementedException();
        }

        public Task<bool?> cfMSG_YesCancel(string cxText, string cxTitle = "Внимание")
        {
            throw new NotImplementedException();
        }

        public Task<bool?> cfMSG_YesCustom(string cxText, string cxCustomButton, string cxTitle = "Внимание")
        {
            throw new NotImplementedException();
        }

        public Task<bool?> cfMSG_YesNo(string cxText, string cxTitle = "Внимание")
        {
            throw new NotImplementedException();
        }

        public Task<bool?> cfMSG_YesNoCancel(string cxText, string cxTitle = "Внимание")
        {
            throw new NotImplementedException();
        }

        public Task<int?> cfMSG_YesNoCustom(string cxText, string cxCustomButton, string cxTitle = "Внимание")
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> cfReceive_DATA_Bytes()
        {
            throw new NotImplementedException();
        }

        public Task<string> cfReceive_DATA_String()
        {
            throw new NotImplementedException();
        }

        public void cfRun_File(string cxFile, string cxArg)
        {
            throw new NotImplementedException();
        }

        public Task cfSend_TCP_DATA(string cxData)
        {
            throw new NotImplementedException();
        }

        public Task cfSend_TCP_DATA(byte[] cxData)
        {
            throw new NotImplementedException();
        }

        public void cfSet_Clipboard(string cxText)
        {
            throw new NotImplementedException();
        }

        public void cfShare_File(string cxFile, string cxTitle)
        {
            throw new NotImplementedException();
        }

        public void cfSTOP_Receive_DATA()
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan?> cfTimeDialog(TimeSpan cxTimeStart, TimeSpan? cxbtn2 = null, TimeSpan? cxbtn3 = null, string cxTitle = null, string cxText = null)
        {
            throw new NotImplementedException();
        }

        public Task cfUpdate()
        {
            throw new NotImplementedException();
        }

        public Task cfUpload()
        {
            throw new NotImplementedException();
        }
    }
}
