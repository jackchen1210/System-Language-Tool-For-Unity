using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;
using System.Collections.Generic;

namespace Core.SystemLanguage
{
    public class FTPTool
    {
        private readonly SystemLanguageFtpManager.DLCFTPInfo dlcInfo;


        public FTPTool(SystemLanguageFtpManager.DLCFTPInfo dlcInfo)
        {
            this.dlcInfo = dlcInfo;
        }

        public async Task UploadFileAsync(SupportedLang supportedLang, TargetType platformType)
        {
            var localSubPath = Path.Combine(supportedLang.ToString().ToUpperInvariant(), Tool.GetFileNameBy(platformType));
            var remoteSubPath = Path.Combine(supportedLang.ToString().ToUpperInvariant(), Tool.GetFTPFileNameBy(platformType));
            var filePath = Path.Combine(SystemLanguageTool.SaveDataPath, localSubPath);
            var url = Path.Combine(dlcInfo.DLCUrl,"Lang", remoteSubPath).Replace("\\","/");
            Debug.Log($"檔案路徑 : {filePath}");
            Debug.Log($"上傳至 : {url}");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UsePassive = true;
            request.KeepAlive = false;
            request.Timeout = 20_000;
            request.Credentials = new NetworkCredential(dlcInfo.DLCUserName, dlcInfo.DLCPassword);
            await Observable.FromCoroutine(_=> ReadFile(request, filePath));

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Debug.Log($"上傳完成 : {response.StatusDescription}");
            }

        }
        /// <summary>
        /// Copy from Editor FTPTool
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filePath"></param>
        /// <param name="uploadFileProgress"></param>
        /// <returns></returns>
        private IEnumerator ReadFile(FtpWebRequest request,string filePath,Action<int> uploadFileProgress = null)
        {
            yield return new WaitForEndOfFrame(); //協程的暫停點
            int FileSize;
            Stream fileStream = File.OpenRead(filePath);
            FileSize = (int)fileStream.Length;
            request.ContentLength = FileSize;
            int buffLength = 2048;
            if (FileSize > 2048)
            {
                buffLength = FileSize / 10;
            }
            using (Stream ftpStream = request.GetRequestStream())
            {
                byte[] buffer = new byte[buffLength];
                int read;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ftpStream.Write(buffer, 0, read);
                    int progress = (int)(fileStream.Position * 100 / FileSize);
                    uploadFileProgress?.Invoke(progress);
                    yield return new WaitForEndOfFrame(); //協程的暫停點
                }
                fileStream.Close();
            }
            yield return new WaitForEndOfFrame(); //協程的暫停點
        }

        public bool CreateFTPDirectory(SupportedLang supportedLang)
        {
            var subPath = supportedLang.ToString().ToUpperInvariant();
            var url = Path.Combine(dlcInfo.DLCUrl, "Lang", subPath).Replace("\\", "/");
            return CreateByUrl(url);
        }
        public bool CreateFTPDirectory()
        {
            var url = Path.Combine(dlcInfo.DLCUrl, "Lang").Replace("\\", "/");
            return CreateByUrl(url);
        }

        private bool CreateByUrl(string url)
        {
            try
            {
                //create the directory
                FtpWebRequest requestDir = (FtpWebRequest)WebRequest.Create(url);
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(dlcInfo.DLCUserName, dlcInfo.DLCPassword);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                requestDir.Timeout = 20_000;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Debug.Log(response.StatusDescription);
                    response.Close();
                    return true;
                }
                else
                {
                    Debug.LogError(ex.Message);
                    Debug.LogError(response.StatusDescription);
                    if(ex.Message.Contains("Unable to connect"))
                    {
                        throw new Exception("請確認是否開啟VPN");
                    }
                    response.Close();
                    return false;
                }
            }
        }

        public async Task UploadManifestFileAsync(string filePath)
        {
            var url = Path.Combine(dlcInfo.DLCUrl,SystemLanguageConfigData.FtpLangFolderName, SystemLanguageConfigData.DlcManifestFullName_FTP).Replace("\\", "/");
            Debug.Log($"檔案路徑 : {filePath}");
            Debug.Log($"上傳至 : {url}");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UsePassive = true;
            request.KeepAlive = false;
            request.Timeout = 20_000;
            request.Credentials = new NetworkCredential(dlcInfo.DLCUserName, dlcInfo.DLCPassword);
            await Observable.FromCoroutine(_ => ReadFile(request, filePath));

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Debug.Log($"上傳完成 : {response.StatusDescription}");
            }

        }
        internal async Task UploadFolderAsync(string folderPath, string ignoreName = "")
        {
            var url = Path.Combine(dlcInfo.DLCUrl, SystemLanguageConfigData.FtpLangFolderName, SystemLanguageConfigData.DlcManifestFullName_FTP).Replace("\\", "/");
            Debug.Log($"資料夾路徑 : {folderPath}");
            await Observable.FromCoroutine(_ => StartUploadFolder(folderPath, ignoreName));

        }

        private IEnumerator StartUploadFolder(string folderPath, string ignoreName)
        {
            yield return new WaitForEndOfFrame(); //協程的暫停點
            bool result = false;
            List<string> fileInfList = new List<string>();
            List<string> DirList = new List<string>(); //NetUtility.GetSubPath(folderPath);
            List<string> DirectoryList = new List<string>();
            DirectoryInfo BaseFolder = new DirectoryInfo($"{folderPath}");
            foreach (var Di in DirList)
            {
                var path = $"{dlcInfo.DLCUrl}/{BaseFolder.Name}/{Di}";
                DirectoryList.Add(path);
                Debug.Log($"添加FTP目錄[{path}]");
            }
            foreach (string subDir in DirectoryList)
            {
                try
                {
                    Debug.Log($"上傳 [{subDir}]");
                    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(subDir);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.UseBinary = true;
                    request.Timeout = 10_000;
                    request.Credentials = new NetworkCredential(dlcInfo.DLCUserName, dlcInfo.DLCPassword);
                    request.UsePassive = true;
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream ftpStream = response.GetResponseStream();
                    ftpStream.Close();
                    response.Close();
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("already exists"))
                    {
                        Debug.Log($"已經有此目錄[{subDir}]");
                    }
                    else
                    {
                        Debug.LogError($"上傳失敗[{ex}]");
                    }
                    //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                }
            }
            yield return new WaitForEndOfFrame(); //協程的暫停點
            foreach (FileInfo dlcFile in BaseFolder.GetFiles("*.*", SearchOption.AllDirectories))
            {
                var path = dlcFile.FullName;
                if (ignoreName.Length > 0 && dlcFile.Name.Contains(ignoreName))
                {
                    Debug.Log($"<color=yellow>略過檔案[{path}]</color>");
                }
                else
                {
                    fileInfList.Add(path);
                    Debug.Log($"添加FTP檔案[{path}]");
                }
            }
            int index = 0;
            foreach (var file in fileInfList)
            {
                // 建立跟FTP溝通的物件
                index++;
                var fileName = file.Replace($"{ BaseFolder.FullName}\\", "").Replace(".bytes", ".eg");
                var target = Path.Combine(dlcInfo.DLCUrl, BaseFolder.Name, fileName);
                Debug.Log($"上傳[{file}] => 目標路徑[{target}]");
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(dlcInfo.DLCUserName, dlcInfo.DLCPassword);
                request.UsePassive = true;
                request.KeepAlive = true;
                int FileSize;
                Stream fileStream = File.OpenRead(file);
                FileSize = (int)fileStream.Length;
                request.ContentLength = FileSize;
                int buffLength = 2048;
                if (FileSize > 2048)
                {
                    buffLength = FileSize / 10;
                }
                using (Stream ftpStream = request.GetRequestStream())
                {
                    byte[] buffer = new byte[buffLength];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ftpStream.Write(buffer, 0, read);
                        int progress = (int)(fileStream.Position * 100 / FileSize);
                        yield return new WaitForEndOfFrame(); //協程的暫停點
                    }
                    fileStream.Close();
                }

                yield return new WaitForEndOfFrame(); //協程的暫停點
                                                      //建立FtpWebResponse物件接收從FTP回應的資料流
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    fileStream.Close();
                }
            }
            result = index == fileInfList.Count;
            Debug.Log($"上傳最終結果[{result}]");
        }
    }
}
