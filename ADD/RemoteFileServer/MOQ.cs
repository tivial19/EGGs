using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.RemoteFileServer
{

    public class czMOQ_RemoteFileServer : czIRemoteFileServer
    {

        public czMOQ_RemoteFileServer()
        {

        }

        public string APK_Path { get; }
        public string Data_Path_Name { get; }
        public ceRFS_Type RFS_Type { get; }

        public Task cfClear_Cache()
        {
            throw new NotImplementedException();
        }

        public Task<string> cfDownload_File_to_Cache(string cxFile_Remote)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> cfGet_Files(string cxPath, Func<string, bool> cxFile_Name_Filter = null, int cxLimit = 0, ceSort_Files cxSort_Type = ceSort_Files.none, bool cxOrderByDescending = false)
        {
            throw new NotImplementedException();
        }

        public string cfGet_Path(params string[] cxDirs)
        {
            throw new NotImplementedException();
        }

        public Task<string> cfGet_UPDATE_APK()
        {
            throw new NotImplementedException();
        }

        public Task<string> cfRead_DATA(string cxData_Type_Name)
        {
            throw new NotImplementedException();
        }

        public Task<string> cfRead_DATA_FILE(string cxFile_Local)
        {
            throw new NotImplementedException();
        }

        public Task<bool> cfRead_Override_DATA_FILE(string cxFile_Local)
        {
            throw new NotImplementedException();
        }

        public Task<bool> cfUploadFileAsync(string cxFile_Local, string cxFile_Rem, bool cxOverride = true)
        {
            throw new NotImplementedException();
        }

        public Task cfWrite_DATA(string cxData, string cxData_Type_Name)
        {
            throw new NotImplementedException();
        }

        public Task cfWrite_DATA_FILE(string cxData_File)
        {
            throw new NotImplementedException();
        }
    }




}
