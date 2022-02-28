using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.RemoteFileServer
{

    public interface czIRemoteFileServer_type
    {
        ceRFS_Type RFS_Type { get; }
    }
    
    
    public interface czIRemoteFileServer_base: czIRemoteFileServer_type
    {
        Task cfCreate_Dir_api(string cxPath, string cxDir_Name);
        Task cfCreate_Disk_api(string oauthKey);
        Task cfDownload_File_api(string cxFile_Remote, string cxFile_Local);
        Task<string[]> cfGet_Files_api(string cxPath, Func<string, bool> cxFile_Name_Filter = null, int cxLimit = 0, ceSort_Files cxSort_Type = ceSort_Files.none, bool cxOrderByDescending = false);
        Task cfUploadFile_api(string cxFile_Local, string cxFile_Rem, bool cxOverride);
    }



}
