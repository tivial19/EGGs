//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using YandexDiskNET;

//namespace cpADD.RemoteFileServer.Yandex
//{


//    public class czYandex_Base_RSarov : czIRemoteFileServer_base
//    {
//        public ceRFS_Type RFS_Type => ceRFS_Type.Yandex_RSarov;


//        YandexDiskRest _DiskApi;

//        public czYandex_Base_RSarov()
//        {

//        }



//        public Task cfCreate_Disk_api(string oauthKey)
//        {
//            return Task.Run(() =>
//            {
//                _DiskApi ??= new YandexDiskRest(oauthKey);
//            });
//        }


//        public Task cfCreate_Dir_api(string cxPath, string cxDir_Name)
//        {

//            return Task.Run(() =>
//            {
//                _DiskApi.CreateFolder(cxPath+ cxDir_Name);
//            });





//            //Resource cxDir_Res = await _DiskApi.MetaInfo.GetInfoAsync(new ResourceRequest() { Path = cxPath, Limit = 100 }, CancellationToken.None);
//            //var Q = cxDir_Res.Embedded.Items.Where(item => item.Name== cxDir_Name && item.Type == ResourceType.Dir);

//            //if (Q != null && !Q.Any()) await _DiskApi.Commands.CreateDictionaryAsync(cxPath+ cxDir_Name);
//        }


//        public Task cfUploadFile_api(string cxFile_Local, string cxFile_Rem, bool cxOverride)
//        {
//            return _DiskApi.UploadResourceAsync(cxFile_Rem, cxFile_Local, cxOverride);
//        }


//        public Task cfDownload_File_api(string cxFile_Remote, string cxFile_Local)
//        {
//            return _DiskApi.DownloadResourceAcync(cxFile_Remote, cxFile_Local);
//        }


//        public Task<string[]> cfGet_Files_api(string cxPath, Func<string, bool> cxFile_Name_Filter = null, int cxLimit = 0, ceSort_Files cxSort_Type = ceSort_Files.none, bool cxOrderByDescending = false)
//        {
//            return Task.Run<string[]>(() =>
//            {
//                SortField? cxSort_Field = cxSort_Type switch
//                {
//                    ceSort_Files.Date_Create => SortField.Created,
//                    ceSort_Files.Date_Modify => SortField.Modified,
//                    ceSort_Files.Name => SortField.Name,
//                    ceSort_Files.Path => SortField.Path,
//                    _ => null
//                };

//                IEnumerable<ResInfo> Qr = _DiskApi.GetResInfo(2000, cxPath, cxSort_Field)._Embedded.Items;
//                if (cxFile_Name_Filter!=null) Qr = Qr.Where(r => cxFile_Name_Filter(r.Name));

//                if (((int)cxSort_Type)>0)
//                {
//                    switch (cxSort_Type)
//                    {
//                        case ceSort_Files.Date_Create:
//                            if (cxOrderByDescending) Qr=Qr.OrderByDescending(s => s.Created); else Qr=Qr.OrderBy(s => s.Created);
//                            break;
//                        case ceSort_Files.Date_Modify:
//                            if (cxOrderByDescending) Qr=Qr.OrderByDescending(s => s.Modified); else Qr=Qr.OrderBy(s => s.Modified);
//                            break;
//                        case ceSort_Files.Name:
//                            if (cxOrderByDescending) Qr=Qr.OrderByDescending(s => s.Name); else Qr=Qr.OrderBy(s => s.Name);
//                            break;
//                        case ceSort_Files.Path:
//                            if (cxOrderByDescending) Qr=Qr.OrderByDescending(s => s.Path); else Qr=Qr.OrderBy(s => s.Path);
//                            break;
//                        default:
//                            break;
//                    }
//                }
//                if (cxLimit>0) Qr=Qr.Take(cxLimit);

//                return Qr.Select(r => r.Path).ToArray();
//            });
//        }




//    }



//}
