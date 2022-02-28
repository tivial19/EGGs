using cpADD.RemoteFileServer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cpADD
{


    public interface czIRemoteFileServer: czIRemoteFileServer_type
    {
        string cfGet_Path(params string[] cxDirs);

        Task<string[]> cfGet_Files(string cxPath, Func<string, bool> cxFile_Name_Filter = null, int cxLimit = 0, ceSort_Files cxSort_Type = ceSort_Files.none, bool cxOrderByDescending = false);
        Task<bool> cfUploadFileAsync(string cxFile_Local, string cxFile_Rem, bool cxOverride = true);


        Task<string> cfDownload_File_to_Cache(string cxFile_Remote);
        Task cfClear_Cache();



        public string APK_Path { get; }

        public string Data_Path_Name { get; }

        Task<string> cfGet_UPDATE_APK();
        Task cfWrite_DATA(string cxData, string cxData_Type_Name);
        Task cfWrite_DATA_FILE(string cxData_File);
        Task<string> cfRead_DATA(string cxData_Type_Name);
        Task<string> cfRead_DATA_FILE(string cxFile_Local);
        Task<bool> cfRead_Override_DATA_FILE(string cxFile_Local);
    }
}
