using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpADD.RemoteFileServer.Yandex
{

    //[czaAutoRegisterSingleInstance]
    public class czYandexDisk_app:czYandexDisk_base, czIRemoteFileServer
    {
       
        czIAPP_MSG _APP_MSG;
        czIAPP_Info _APP_Info;

        bool isDirs_Created = false;


        public string APK_Path { get; }
        public string Data_Path { get; }
        public string Data_Path_Name { get; }
        

        public czYandexDisk_app(czIAPP cxAPP, czIRemoteFileServer_base cxRFS_Base) : base(cxRFS_Base, cxAPP.Path_Cache)
        {
            _APP_MSG = cxAPP;
            _APP_Info = cxAPP;

            APK_Path = cfGet_Path("apk");
            Data_Path = cfGet_Path(APK_Path, "Data");
            Data_Path_Name = cfGet_Path(Data_Path, _APP_Info.Name);
        }




        public async Task cfCreate_Dirs()
        {
            if (!isDirs_Created)
            {
                await cfCreate_Dir(Data_Path_Name);
                isDirs_Created = true;
            }
        }




        public Task cfWrite_DATA(string cxData, string cxData_Type_Name)
        {
            string cxFile = Path.Combine(_APP_Info.Path_Cache, cxData_Type_Name, ".txt");
            if (File.Exists(cxFile)) File.Delete(cxFile);
            File.WriteAllText(cxFile, cxData, Encoding.UTF8);
            return cfWrite_DATA_FILE(cxFile);
        }

        public Task cfWrite_DATA_FILE(string cxData_File)
        {
            return Task.Run(async () =>
            {
                string cxFileName = Path.GetFileNameWithoutExtension(cxData_File);
                string cxName = cfGetYname(cxFileName);
                string cxFile_Rem = Path.Combine(Data_Path_Name, cxName + Path.GetExtension(cxData_File));
                if (await cfUploadFileAsync(cxData_File, cxFile_Rem)) _APP_MSG.cfMSG_Info($"{cxName} файл сохранен.", true);
            });

            static string cfGetYname(string cxOrg_FileName)
            {
                if (string.IsNullOrWhiteSpace(cxOrg_FileName)) throw new Exception("Yandex cfWrite_FILE cfGetYname cxOrg_FileName is empty!");
                //string cxR = $"{cxOrg_FileName}_{DateTime.Now.ToString("yy.MM.dd_hh.mm.ss")}";
                string cxR = $"{cxOrg_FileName}_{DateTime.Now.ToString("dd.MM.yy_HHmmss")}";
                return cxR;
            }
        }




        public async Task<string> cfRead_DATA(string cxData_Type_Name)
        {
            string cxFile = Path.Combine(_APP_Info.Path_Cache, cxData_Type_Name, ".txt");
            if (File.Exists(cxFile)) File.Delete(cxFile);
            string cxFile_New = await cfRead_DATA_FILE(cxFile);
            if (!string.IsNullOrWhiteSpace(cxFile_New)) return File.ReadAllText(cxFile_New, Encoding.UTF8);
            return null;
        }


        public async Task<bool> cfRead_Override_DATA_FILE(string cxFile_Local)
        {
            string cxFile = await cfRead_DATA_FILE(cxFile_Local);
            if (!string.IsNullOrWhiteSpace(cxFile) && File.Exists(cxFile))
            {
                if (true==await _APP_MSG.cfMSG_YesCancel("Данные будут перезаписаны. Продолжить?"))
                {
                    File.Copy(cxFile, cxFile_Local, true);
                    return true;
                }
            }
            return false;
        }

        public async Task<string> cfRead_DATA_FILE(string cxFile_Local)
        {
            string cxFileName = Path.GetFileNameWithoutExtension(cxFile_Local);
            string cxFileName_New = null, cxDate_Filter = null, cxSelect_Date = "по дате";
            IEnumerable<string> Qf = null;
            do
            {
                var Qa = await cfGet_Files(Data_Path_Name, s => s.StartsWith(cxFileName), 15, ceSort_Files.Date_Create, true);
                if (Qa?.Any() ?? false)
                {
                    IEnumerable<string> cxFiles_Select = Qa.Select(s => Path.GetFileNameWithoutExtension(s));
                    cxFileName_New = await _APP_MSG.cfMSG_Select(cxFiles_Select.ToArray(),cxClear: cxSelect_Date);
                    if (cxFileName_New == null) return null;
                    if (cxFileName_New == cxSelect_Date)
                    {
                        DateTime? cxDate = await _APP_MSG.cfDateDialog(DateTime.Today);
                        if (cxDate == null) return null;
                        cxDate_Filter = cxDate.Value.ToString("dd.MM.yy");
                    }
                    else Qf = Qa.Where(s => Path.GetFileNameWithoutExtension(s) == cxFileName_New);
                }
                else
                {
                    await _APP_MSG.cfMSG_OK("Данных на найдено.", "Ошибка при загрузке данных!");
                    return null;
                }
            } while (Qf == null);

            if (Qf?.Any() ?? false) return await cfDownload_File_to_Cache(Qf.Single());
            else await _APP_MSG.cfMSG_OK($"Файлов с именем {cxFileName_New} не найдено!");
            return null;
        }







        public async Task<string> cfGet_UPDATE_APK()
        {
            string cxPath = APK_Path;
            IEnumerable<string> Qf;// = await _RFS.cfGet_Files(RFS_Path);
            if ((Qf = await cfGet_Files(cxPath)) != null)
            {
                var Qrn = Qf.Where(s => System.IO.Path.GetFileNameWithoutExtension(s).StartsWith(_APP_Info.Package_Name)).Where(s => System.IO.Path.GetExtension(s).ToLower() == ".apk");
                if (Qrn.Any())
                {
                    string cxFile;
                    if (Qrn.Count() > 1)
                    {
                        string cxSelect = await _APP_MSG.cfMSG_Select(Qrn.Select(s => System.IO.Path.GetFileNameWithoutExtension(s)).ToArray());
                        if (cxSelect == null) return null;
                        cxFile = Qrn.Where(s => System.IO.Path.GetFileNameWithoutExtension(s) == cxSelect).First();
                    }
                    else cxFile = Qrn.Single();

                    if (true == await _APP_MSG.cfMSG_YesNo($"Обновление {System.IO.Path.GetFileNameWithoutExtension(cxFile)} будет скачано. Продолжить?"))
                    {
                        string cxFileLocal = await cfDownload_File_to_Cache(cxFile);
                        if (cxFileLocal != null)
                        {
                            _APP_MSG.cfMSG_Info("Обновление скачано начинаем установку.", true);
                            return cxFileLocal;
                        }
                    }
                    return null;
                }
                else throw new Exception("Обновление не найдено.");
            }
            else throw new Exception($"Ошибка полученния данных по пути {cxPath}");
        }









        protected override async Task cfCreate_Disk(bool cxClear_Cache)
        {
            await base.cfCreate_Disk(cxClear_Cache);
            await cfCreate_Dirs(); 
        }



    }
}
