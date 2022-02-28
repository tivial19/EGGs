//#if HAVE_YandexDisk

using cpADD;
using cpADD.Ability;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cpADD.RemoteFileServer.Yandex
{


    
    public class czYandexDisk_base 
    {
        const string OauthToken = "AgAAAAAVwOVxAAYlMXngR-9hB0ZkvW-WIEtmOfQ";
        const string Path_Slash = "/";
        readonly string _CachePath;

        czIRemoteFileServer_base _RFS_Base;

        public ceRFS_Type RFS_Type => _RFS_Base.RFS_Type;

        public czYandexDisk_base(czIRemoteFileServer_base cxRFS_Base, string cxCachePath)
        {
            _RFS_Base=cxRFS_Base;
            _CachePath=cxCachePath;
        }



        public async Task<string[]> cfGet_Files(string cxPath, Func<string, bool> cxFile_Name_Filter = null, int cxLimit = 0, ceSort_Files cxSort_Type = ceSort_Files.none, bool cxOrderByDescending = false)
        {
            try
            {
                await cfCreate_Disk(false);
                return await _RFS_Base.cfGet_Files_api(cxPath, cxFile_Name_Filter, cxLimit, cxSort_Type, cxOrderByDescending);
            }
            catch (Exception cxE)
            {
                throw new Exception("Ошибка при получении списка файлов: " + cxE.Message);
            }
        }



        public async Task<bool> cfUploadFileAsync(string cxFile_Local, string cxFile_Rem, bool cxOverride = true)
        {
            try
            {
                await cfCreate_Disk(false);
                await _RFS_Base.cfUploadFile_api(cxFile_Local, cxFile_Rem, cxOverride);
                return true;
            }
            catch (Exception cxE)
            {
                throw new Exception("Ошибка при выгрузке файла: " + cxE.Message);
            }
        }


        public async Task<string> cfDownload_File_to_Cache(string cxFile_Remote)
        {
            try
            {
                await cfCreate_Disk(true);
                if (!Directory.Exists(_CachePath)) Directory.CreateDirectory(_CachePath);//only for windows
                string cxFile_Local = Path.Combine(_CachePath, Path.GetFileName(cxFile_Remote));
                if (File.Exists(cxFile_Local)) await cfClear_Cache();
                await _RFS_Base.cfDownload_File_api(cxFile_Remote, cxFile_Local);
                return cxFile_Local;
            }
            catch (Exception cxE)
            {
                throw new Exception("Ошибка при загрузке файла: " + cxE.Message);
            }
        }











 










        public string cfGet_Path(params string[] cxDirs)
        {
            string cxPath = null;
            foreach (string cxDir in cxDirs)
            {
                cxPath+=cxDir;
                if (!cxDir.EndsWith(Path_Slash)) cxPath += Path_Slash;
            }
            return cxPath;
        }


        public Task cfClear_Cache()
        {
            return Task.Run(() =>
            {
                if (Directory.Exists(_CachePath))
                {
                    var Qf = Directory.GetFiles(_CachePath);
                    foreach (var cxFile in Qf) File.Delete(cxFile);
                }
            });
        }








        protected virtual async Task cfCreate_Disk(bool cxClear_Cache)
        {
            await _RFS_Base.cfCreate_Disk_api(OauthToken);
            if (cxClear_Cache) await cfClear_Cache();
        }


        protected async Task cfCreate_Dir(string cxPath)
        {
            if (cxPath == Path.GetPathRoot(cxPath)) return;
            List<string> cxDirs_to_Create = cfGet_Dir_Pathes(cxPath);
            foreach (var cxDir in cxDirs_to_Create) await cfCreate(cxDir);

            async Task cfCreate(string cxPath)
            {
                string cxParent_Path = cfGet_Parent_Path(cxPath);
                string cxDir_Name = cfGet_DirName(cxPath);

                await _RFS_Base.cfCreate_Dir_api(cxParent_Path, cxDir_Name);
            }



            string[] cfGet_DirNames(string cxPath)
            {
                return cxPath.Split(Path_Slash, StringSplitOptions.RemoveEmptyEntries); ;
            }


            string cfGet_DirName(string cxPath)
            {
                string[] cxD = cfGet_DirNames(cxPath);
                return cxD.Last();
            }

            string cfGet_Parent_Path(string cxPath)
            {
                string[] cxD = cfGet_DirNames(cxPath);
                if (cxD.Length == 1) return Path_Slash;

                var Q = cxD.Take(cxD.Length - 1);
                return cfGet_Path(Q.ToArray());
            }

            List<string> cfGet_Dir_Pathes(string cxPath)
            {
                string[] cxDirs = cfGet_DirNames(cxPath);
                List<string> cxPaths = new List<string>();

                string cxCurPath = Path_Slash;
                for (int i = 0; i < cxDirs.Length; i++)
                {
                    cxCurPath += cxDirs[i] + Path_Slash;
                    cxPaths.Add(cxCurPath);
                }
                return cxPaths;
            }

        }






    }


}

//#endif