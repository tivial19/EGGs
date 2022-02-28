using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace cpXamarin_APP
{
    public partial class czAPP 
    {


        public  async Task cfUpdate()
        {
            string cxFileNew = await _RFS.cfGet_UPDATE_APK();
            if (cxFileNew != null) cfRun_File(cxFileNew);
        }


        public  async Task cfUpload()
        {
            var cxFs = Directory.GetFiles(Path_Download, "*.apk");
            if (cxFs.Any())
            {
                var cxFp = cxFs.Where(s => System.IO.Path.GetFileNameWithoutExtension(s) == Package_Name);
                string cxFile_Upload;
                if (cxFp.Any()) cxFile_Upload = cxFp.Single();
                else cxFile_Upload = cxFs.First();
                string cxFile_Name = System.IO.Path.GetFileName(cxFile_Upload);

                if (true==await cfMSG_YesNo($"Загрузить файл {cxFile_Name}?"))
                    if(await _RFS.cfUploadFileAsync(cxFile_Upload, _RFS.APK_Path + cxFile_Name)) cfMSG_Info($"Файл {cxFile_Name} был успешно загружен.", true);
            }
            else throw new Exception("Файлов для загрузки не найдено. " + Path_Download);
        }








    }



}
