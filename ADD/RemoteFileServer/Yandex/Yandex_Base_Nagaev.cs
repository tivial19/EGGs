using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YandexDisk.Client;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;


namespace cpADD.RemoteFileServer.Yandex
{


    public class czYandex_Base_Nagaev: czIRemoteFileServer_base
    {
        public ceRFS_Type RFS_Type => ceRFS_Type.Yandex_Nagaev;

        IDiskApi _DiskApi;

        public czYandex_Base_Nagaev()
        {

        }



        public Task cfCreate_Disk_api(string oauthKey)
        {
            return Task.Run(() =>
            {
                _DiskApi ??= new DiskHttpApi(oauthKey);

            });
        }


        public async Task cfCreate_Dir_api(string cxPath, string cxDir_Name)
        {
            Resource cxDir_Res = await _DiskApi.MetaInfo.GetInfoAsync(new ResourceRequest() { Path = cxPath, Limit = 100 }, CancellationToken.None);
            var Q = cxDir_Res.Embedded.Items.Where(item => item.Name== cxDir_Name && item.Type == ResourceType.Dir);

            if (Q != null && !Q.Any()) await _DiskApi.Commands.CreateDictionaryAsync(cxPath+ cxDir_Name);
        }


        public Task cfUploadFile_api(string cxFile_Local, string cxFile_Rem, bool cxOverride)
        {
            return _DiskApi.Files.UploadFileAsync(cxFile_Rem, cxOverride, cxFile_Local, CancellationToken.None);
        }


        public Task cfDownload_File_api(string cxFile_Remote, string cxFile_Local)
        {
            return _DiskApi.Files.DownloadFileAsync(cxFile_Remote, cxFile_Local);
        }


        public async Task<string[]> cfGet_Files_api(string cxPath, Func<string, bool> cxFile_Name_Filter = null, int cxLimit = 0, ceSort_Files cxSort_Type = ceSort_Files.none, bool cxOrderByDescending = false)
        {
            Resource cxItems_Dir_Res = await _DiskApi.MetaInfo.GetInfoAsync(new ResourceRequest() { Path = cxPath, Limit = 2000 }, CancellationToken.None);
            var Qr = cxItems_Dir_Res.Embedded.Items.Where(item => item.Type == ResourceType.File);
            if (cxFile_Name_Filter!=null) Qr = Qr.Where(s => cxFile_Name_Filter(s.Name));

            if (((int)cxSort_Type)>0)
            {
                switch (cxSort_Type)
                {
                    case ceSort_Files.Date_Create:
                        if (cxOrderByDescending) Qr=Qr.OrderByDescending(s => s.Created); else Qr=Qr.OrderBy(s => s.Created);
                        break;
                    case ceSort_Files.Date_Modify:
                        if (cxOrderByDescending) Qr=Qr.OrderByDescending(s => s.Modified); else Qr=Qr.OrderBy(s => s.Modified);
                        break;
                    case ceSort_Files.Name:
                        if (cxOrderByDescending) Qr=Qr.OrderByDescending(s => s.Name); else Qr=Qr.OrderBy(s => s.Name);
                        break;
                    case ceSort_Files.Path:
                        if (cxOrderByDescending) Qr=Qr.OrderByDescending(s => s.Path); else Qr=Qr.OrderBy(s => s.Path);
                        break;
                    default:
                        break;
                }
            }
            if (cxLimit>0) Qr=Qr.Take(cxLimit);
            return Qr.Select(s => s.Path).ToArray();
        }




    }



}
