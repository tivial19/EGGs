using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace cpADD
{

    //public interface czIAPP_All : czIAPP, czIAPP_MSG, czIAPP_base, czIAPP_Info
    //{

    //}

    public interface czIAPP:czIAPP_MSG, czIAPP_base, czIAPP_Info
    {
        Task cfUpdate();
        Task cfUpload();

        void cfRun_File(string cxFile, string cxArg);
        void cfShare_File(string cxFile, string cxTitle);
        void cfSet_Clipboard(string cxText);
        Task cfSend_TCP_DATA(string cxData);
        Task cfSend_TCP_DATA(byte[] cxData);
        Task<string> cfReceive_DATA_String();
        Task<byte[]> cfReceive_DATA_Bytes();
        void cfSTOP_Receive_DATA();
    }


    public interface czIAPP_Thread
    {
        void cfMainThread(Action cxAction);
        Task cfMainThread(Func<Task> cxFunc);
        Task<T> cfMainThread<T>(Func<Task<T>> cxFunc);
    }

    public interface czIAPP_MSG
    {
        void cfMSG_Info(string cxText, bool cxLength_Long);
        Task<bool?> cfMSG_OK(string cxText, string cxTitle = "Внимание");
        Task<bool?> cfMSG_YesNo(string cxText, string cxTitle = "Внимание");
        Task<bool?> cfMSG_YesCancel(string cxText, string cxTitle = "Внимание");
        Task<bool?> cfMSG_YesNoCancel(string cxText, string cxTitle = "Внимание");
        Task<bool?> cfMSG_YesCustom(string cxText, string cxCustomButton, string cxTitle = "Внимание");
        Task<int?> cfMSG_YesNoCustom(string cxText, string cxCustomButton, string cxTitle = "Внимание");
        Task<string> cfMSG_Select(string[] cxToSelect, string cxTitle = "Сделай свой выбор:", string cxCancel = "Отмена", string cxClear = "");
        Task<string> cfMSG_Select_File(string cxFile_Filter = null, string cxDir = null);
        Task<string> cfMSG_Input(string cxText, string cxTitle = "Введите значение:", string cxPlaceHolder = null, string cxOK = "OK", string cxCancel = "Отмена", bool cxKeyBoardText = false, string cxInitialValue = "");
        Task<DateTime?> cfDateDialog(DateTime cxDateStart, DateTime? cxbtn2 = null, DateTime? cxbtn3 = null, string cxTitle = null, string cxText = null);
        Task<TimeSpan?> cfTimeDialog(TimeSpan cxTimeStart, TimeSpan? cxbtn2 = null, TimeSpan? cxbtn3 = null, string cxTitle = null, string cxText = null);
        Task<int?> cfMSG_Universal(string cxText, string cxButton1 = "Да", string cxButton2 = "Нет", string cxButton0 = "Отмена", string cxTitle = "Внимание");
        Task<(bool isSelected, T Value)> cfMSG_Select_Object<T>(T[] cxObjects, Func<T, string> cxGet_Text, string cxTitle = "Сделай свой выбор:", string cxCancel = "Отмена", string cxClear = "");
    }








    public interface czIAPP_base: czIAPP_Thread
    {
        event Func<Task> ceAppShutDown;

        bool isShutDowning { get; }

        void cfExit();
        Task cfSAVE_ALL();

        Task cfSave_APP_params();
        Task<bool> cfis_ShutDown();
        void cfShutDown();


        //void cfShow_Exception(string cxText, string cxTitle);
        //string cfGet_Message_Exception(Exception cxE);
    }







    public interface czIAPP_Info
    {
        TimeSpan Time_from_Start { get; }

        string Path_App { get; }
        string Path_Cache { get; }
        string Path_Download { get; }

        string[] Shared_PRJ_Info { get; }

        string Name { get; }
        string Version { get; }
        string Package_Name { get; }

        string Title { get; }
        string Year { get; }
        string Device_Name { get; }

    }




}
