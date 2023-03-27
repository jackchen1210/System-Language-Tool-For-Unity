using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
//using BestHTTP;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.SystemLanguage
{
    public class SystemLanguageToolForApp
    {
        public async Task<SystemLanguageDlcManifest> GetManifestAsync(string domainUrl)
        {
            var fileUrl = Path.Combine(domainUrl, "DLC", SystemLanguageConfigData.FtpLangFolderName, SystemLanguageConfigData.DlcManifestFullName_FTP).Replace("\\", "/");
            PrintLog(fileUrl);
            var jsonData = string.Empty;
            try
            {
                //jsonData = await new HTTPRequest(new Uri(fileUrl), HTTPMethods.Get).GetAsStringAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"取得多語系文字 Manifest失敗 \n" +
                    $" {fileUrl}\n" +
                    $"{e.Message}");
                throw;
            }

            PrintLog($"remote manifest : {jsonData}");
            return JsonConvert.DeserializeObject<SystemLanguageDlcManifest>(jsonData);
        }

        public async Task<SystemLanguageData> GetFileAsync(string domainUrl, TargetType targetType, SupportedLang supportedLang)
        {
            var fileUrl = Path.Combine(domainUrl, "DLC", SystemLanguageConfigData.FtpLangFolderName, supportedLang.ToString(), ToolForApp.GetFTPFileNameBy(targetType)).Replace("\\", "/");
            PrintLog($"fileUrl : {fileUrl}");
            var jsonData = string.Empty; //await new HTTPRequest(new Uri(fileUrl), HTTPMethods.Get).GetAsStringAsync();
            PrintLog($"rcv data : {jsonData}");
            return new SystemLanguageData { Datas = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData) };
        }
        private void PrintLog(string data)
        {
            Debug.Log($"<color=yellow>[SystemLanguageToolForApp]</color> {data}");
        }

    }
}
