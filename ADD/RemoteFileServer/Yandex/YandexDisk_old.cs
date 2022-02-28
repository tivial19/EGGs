////#if HAVE_YandexDisk

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using YandexDisk.Client;
//using YandexDisk.Client.Clients;
//using YandexDisk.Client.Http;
//using YandexDisk.Client.Protocol;

//namespace cpADD.RemoteFileServer
//{



//    public class czYandexDisk : czIRemoteFileServer
//    {
//        const string OauthToken = "AgAAAAAVwOVxAAYlMXngR-9hB0ZkvW-WIEtmOfQ";
//        const string Path_Slash = "/";
//        string CachePath;

//        bool isDirs_Created = false;

//        IDiskApi _DiskApi;

//        czIAPP_MSG _APP_MSG;
//        czIAPP_Info _APP_Info;


//        public string APK_Path { get; }
//        public string Data_Path { get; }
//        public string Data_Path_Name { get; }



//        public czYandexDisk(czIAPP cxAPP)
//        {
//            _APP_MSG = cxAPP;
//            _APP_Info = cxAPP;

//            CachePath = _APP_Info.Path_Cache;

//            APK_Path = cfGet_Path("apk");
//            Data_Path = cfGet_Path(APK_Path, "Data");
//            Data_Path_Name = cfGet_Path(Data_Path, _APP_Info.Name);
//            //cfSet_Data_Dir();
//        }


//        //private void cfSet_Data_Dir(string cxName=null)
//        //{
//        //    if (string.IsNullOrWhiteSpace(cxName)) cxName = _APP_Info.Name;
//        //    Data_Path_Name = cfGet_Path(Data_Path, cxName);
//        //}





//        private async Task cfCreate_Disk(bool cxClear_Cache)
//        {
//            _DiskApi ??= new DiskHttpApi(OauthToken);
//            await cfCreate_Dirs();
//            if (cxClear_Cache) await cfClear_Cache();
//        }

//        public Task cfClear_Cache()
//        {
//            return Task.Run(() =>
//            {
//                if (Directory.Exists(CachePath))
//                {
//                    var Qf = Directory.GetFiles(CachePath);
//                    foreach (var cxFile in Qf) File.Delete(cxFile);
//                }
//            });
//        }


//        public async Task cfCreate_Dirs()
//        {
//            if (!isDirs_Created)
//            {
//                await cfCreate_Dir(Data_Path_Name);
//                isDirs_Created = true;
//            }
//        }


//        private async Task cfCreate_Dir(string cxPath)
//        {
//            if (cxPath == Path.GetPathRoot(cxPath)) return;
//            List<string> cxDirs_to_Create = cfGet_Dir_Pathes(cxPath);
//            foreach (var cxDir in cxDirs_to_Create) await cfCreate(cxDir);

//            async Task cfCreate(string cxPath)
//            {
//                string cxParent_Path = cfGet_Parent_Path(cxPath);
//                string cxDir_Name = cfGet_DirName(cxPath);

//                Resource cxDir_Res = await _DiskApi.MetaInfo.GetInfoAsync(new ResourceRequest() { Path = cxParent_Path, Limit = 100 }, CancellationToken.None);
//                var Q = cxDir_Res.Embedded.Items.Where(item => item.Name== cxDir_Name && item.Type == ResourceType.Dir);

//                if (Q != null && !Q.Any())
//                    await _DiskApi.Commands.CreateDictionaryAsync(cxPath);
//            }



//            string[] cfGet_DirNames(string cxPath)
//            {
//                return cxPath.Split(Path_Slash, StringSplitOptions.RemoveEmptyEntries); ;
//            }


//            string cfGet_DirName(string cxPath)
//            {
//                string[] cxD = cfGet_DirNames(cxPath);
//                return cxD.Last();
//            }

//            string cfGet_Parent_Path(string cxPath)
//            {
//                string[] cxD = cfGet_DirNames(cxPath);
//                if (cxD.Length == 1) return Path_Slash;

//                var Q = cxD.Take(cxD.Length - 1);
//                return cfGet_Path(Q.ToArray());
//            }

//            List<string> cfGet_Dir_Pathes(string cxPath)
//            {
//                string[] cxDirs = cfGet_DirNames(cxPath);
//                List<string> cxPaths = new List<string>();

//                string cxCurPath = Path_Slash;
//                for (int i = 0; i < cxDirs.Length; i++)
//                {
//                    cxCurPath += cxDirs[i] + Path_Slash;
//                    cxPaths.Add(cxCurPath);
//                }
//                return cxPaths;
//            }




//        }




//        public string cfGet_Path(params string[] cxDirs)
//        {
//            string cxPath = null;
//            foreach (string cxDir in cxDirs)
//            {
//                if (!cxDir.StartsWith(Path_Slash))
//                {
//                    if (cxPath == null || !cxPath.EndsWith(Path_Slash)) cxPath += Path_Slash;
//                }
//                cxPath += cxDir;
//            }

//            if (!cxPath.EndsWith(Path_Slash)) cxPath += Path_Slash;
//            return cxPath;
//        }




//        public async Task<IEnumerable<string>> cfGet_Files(string cxPath, bool cxOderByDate = false, string cxName_Filter = null, int? cxLimit_MY = null)
//        {
//            var Qr = await cfGet_File_Resource(cxPath);
//            if (cxName_Filter != null) Qr = Qr.Where(s => s.Name.Contains(cxName_Filter));
//            if (cxOderByDate) Qr = Qr.OrderByDescending(s => s.Created);
//            if (cxLimit_MY != null) Qr = Qr.Take(cxLimit_MY.Value);
//            return Qr.Select(s => s.Path);
//        }



//        public async Task<bool> cfUploadFileAsync(string cxFile_Local, string cxFile_Rem, bool cxOverride = true)
//        {
//            try
//            {
//                await cfCreate_Disk(false);
//                await _DiskApi.Files.UploadFileAsync(cxFile_Rem, cxOverride, cxFile_Local, CancellationToken.None);
//                return true;
//            }
//            catch (Exception cxE)
//            {
//                throw new Exception("Ошибка при выгрузке файла: " + cxE.Message);
//            }
//        }


//        public async Task<string> cfDownload_File_to_Cache(string cxFile_Remote)
//        {
//            try
//            {
//                await cfCreate_Disk(true);
//                if (!Directory.Exists(CachePath)) Directory.CreateDirectory(CachePath);//only for windows
//                string cxFile_Local = Path.Combine(CachePath, Path.GetFileName(cxFile_Remote));
//                if (File.Exists(cxFile_Local)) await cfClear_Cache();
//                await _DiskApi.Files.DownloadFileAsync(cxFile_Remote, cxFile_Local);
//                return cxFile_Local;
//            }
//            catch (Exception cxE)
//            {
//                throw new Exception("Ошибка при загрузке файла: " + cxE.Message);
//            }
//        }









//        public async Task<string> cfGet_UPDATE_APK()
//        {
//            string cxPath = APK_Path;
//            IEnumerable<string> Qf;// = await _RFS.cfGet_Files(RFS_Path);
//            if ((Qf = await cfGet_Files(cxPath)) != null)
//            {
//                var Qrn = Qf.Where(s => System.IO.Path.GetFileNameWithoutExtension(s).StartsWith(_APP_Info.Package_Name)).Where(s => System.IO.Path.GetExtension(s).ToLower() == ".apk");
//                if (Qrn.Any())
//                {
//                    string cxFile;
//                    if (Qrn.Count() > 1)
//                    {
//                        string cxSelect = await _APP_MSG.cfMSG_Select(Qrn.Select(s => System.IO.Path.GetFileNameWithoutExtension(s)).ToArray());
//                        if (cxSelect == null) return null;
//                        cxFile = Qrn.Where(s => System.IO.Path.GetFileNameWithoutExtension(s) == cxSelect).First();
//                    }
//                    else cxFile = Qrn.Single();

//                    if (true == await _APP_MSG.cfMSG_YesNo($"Обновление {System.IO.Path.GetFileNameWithoutExtension(cxFile)} будет скачано. Продолжить?"))
//                    {
//                        string cxFileLocal = await cfDownload_File_to_Cache(cxFile);
//                        if (cxFileLocal != null)
//                        {
//                            _APP_MSG.cfMSG_Info("Обновление скачано начинаем установку.",true);
//                            return cxFileLocal;
//                        }
//                    }
//                    return null;
//                }
//                else throw new Exception("Обновление не найдено.");
//            }
//            else throw new Exception($"Ошибка полученния данных по пути {cxPath}");
//        }








//        public Task cfWrite_DATA(string cxData, string cxData_Type_Name)
//        {
//            string cxFile = Path.Combine(_APP_Info.Path_Cache, cxData_Type_Name, ".txt");
//            if (File.Exists(cxFile)) File.Delete(cxFile);
//            File.WriteAllText(cxFile, cxData, Encoding.UTF8);
//            return cfWrite_DATA_FILE(cxFile);
//        }

//        public Task cfWrite_DATA_FILE(string cxData_File)
//        {
//            return Task.Run(async () =>
//           {
//               string cxFileName = Path.GetFileNameWithoutExtension(cxData_File);
//               string cxName = cfGetYname(cxFileName);
//               string cxFile_Rem = Path.Combine(Data_Path_Name, cxName + Path.GetExtension(cxData_File));
//               if (await cfUploadFileAsync(cxData_File, cxFile_Rem)) _APP_MSG.cfMSG_Info($"{cxName} файл сохранен.", true);
//           });

//            static string cfGetYname(string cxOrg_FileName)
//            {
//                if (string.IsNullOrWhiteSpace(cxOrg_FileName)) throw new Exception("Yandex cfWrite_FILE cfGetYname cxOrg_FileName is empty!");
//                //string cxR = $"{cxOrg_FileName}_{DateTime.Now.ToString("yy.MM.dd_hh.mm.ss")}";
//                string cxR = $"{cxOrg_FileName}_{DateTime.Now.ToString("dd.MM.yy_HHmmss")}";
//                return cxR;
//            }
//        }




//        public async Task<string> cfRead_DATA(string cxData_Type_Name)
//        {
//            string cxFile = Path.Combine(_APP_Info.Path_Cache, cxData_Type_Name, ".txt");
//            if (File.Exists(cxFile)) File.Delete(cxFile);
//            string cxFile_New = await cfRead_DATA_FILE(cxFile);
//            if (!string.IsNullOrWhiteSpace(cxFile_New)) return File.ReadAllText(cxFile_New, Encoding.UTF8);
//            return null;
//        }


//        public async Task<bool> cfRead_Override_DATA_FILE(string cxFile_Local)
//        {
//            string cxFile = await cfRead_DATA_FILE(cxFile_Local);
//            if (!string.IsNullOrWhiteSpace(cxFile) && File.Exists(cxFile))
//            {
//                if (true==await _APP_MSG.cfMSG_YesCancel("Данные будут перезаписаны. Продолжить?"))
//                {
//                    File.Copy(cxFile, cxFile_Local, true);
//                    return true;
//                }
//            }
//            return false;
//        }

//        public async Task<string> cfRead_DATA_FILE(string cxFile_Local)
//        {
//            string cxFileName = Path.GetFileNameWithoutExtension(cxFile_Local);
//            string cxFileName_New = null, cxDate_Filter = null, cxSelect_Date = "по дате";
//            IEnumerable<string> Qf = null;
//            do
//            {
//                var Qa = await cfGet_Files_Data();
//                if (Qa?.Any() ?? false)
//                {
//                    IEnumerable<string> cxFiles_Select = Qa.Select(s => Path.GetFileNameWithoutExtension(s));
//                    cxFileName_New = await _APP_MSG.cfMSG_Select(cxFiles_Select.ToArray(), cxClear: cxSelect_Date);
//                    if (cxFileName_New == null) return null;
//                    if (cxFileName_New == cxSelect_Date)
//                    {
//                        DateTime? cxDate = await _APP_MSG.cfDateDialog(DateTime.Today);
//                        if (cxDate == null) return null;
//                        cxDate_Filter = cxDate.Value.ToString("dd.MM.yy");
//                    }
//                    else Qf = Qa.Where(s => Path.GetFileNameWithoutExtension(s) == cxFileName_New);
//                }
//                else
//                {
//                    await _APP_MSG.cfMSG_OK("Данных на найдено.", "Ошибка при загрузке данных!");
//                    return null;
//                }
//            } while (Qf == null);

//            if (Qf?.Any() ?? false) return await cfDownload_File_to_Cache(Qf.Single());
//            else await _APP_MSG.cfMSG_OK($"Файлов с именем {cxFileName_New} не найдено!");
//            return null;



//            async Task<IEnumerable<string>> cfGet_Files_Data()
//            {
//                var Qr = await cfGet_File_Resource(Data_Path_Name);
//                Qr = Qr.Where(s => s.Name.StartsWith(cxFileName));
//                if (cxDate_Filter != null) Qr = Qr.Where(s => s.Name.Contains(cxDate_Filter));
//                return Qr.OrderByDescending(s => s.Created).Take(15).Select(s => s.Path);
//            }

//        }

















//        private async Task<IEnumerable<Resource>> cfGet_File_Resource(string cxPath)
//        {
//            try
//            {
//                await cfCreate_Disk(false);
//                Resource cxItems_Dir_Res = await _DiskApi.MetaInfo.GetInfoAsync(new ResourceRequest() { Path = cxPath, Limit = 2000 }, CancellationToken.None);
//                return cxItems_Dir_Res.Embedded.Items.Where(item => item.Type == ResourceType.File);
//            }
//            catch (Exception cxE)
//            {
//                throw new Exception("Ошибка при получении списка файлов: " + cxE.Message);
//            }
//        }






//    }



//}

////#endif