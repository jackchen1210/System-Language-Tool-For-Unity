#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.SystemLanguage
{
    public class SystemLanguageTool
    {
        public static string SaveDataPath => SystemLanguageConfigData.SaveDataPath;
        private string fileName = SystemLanguageConfigData.PlatformFileFullName;
        private string remoteUrl;
        private HttpClient httpClient;
        private string targetLang = nameof(SupportedLang.ZH_TW);
        private string targetSysLangPath;

        public SystemLanguageTool(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task RunAsync()
        {
            var data = await PullDataAsync();
            await SaveDataAsync(Tool.ParseRawDataFromCsv(data));
            ReplaceFile();
        }

        public SystemLanguageTool SetTargetSysLangPath(string targetSysLangPath)
        {
            this.targetSysLangPath = targetSysLangPath;
            return this;
        }
        public SystemLanguageTool SetFileName(string fileName)
        {
            this.fileName = fileName;
            return this;
        }

        public SystemLanguageTool SetCurrentLanguage(SupportedLang supportedLang)
        {
            switch (supportedLang)
            {
                case SupportedLang.ZH_TW:
                    targetLang = nameof(SupportedLang.ZH_TW);
                    break;
                case SupportedLang.EN_US:
                    targetLang = nameof(SupportedLang.EN_US);
                    break;
                default:
                    targetLang = nameof(SupportedLang.ZH_TW);
                    break;
            }
            return this;
        }

        public SystemLanguageTool SetRemoteUrl(string remoteUrl)
        {
            this.remoteUrl = remoteUrl;
            return this;
        }

        public async Task<string> PullDataAsync()
        {
            if (httpClient == null)
            {
                Debug.LogError("no httpClient available");
                return string.Empty;
            }

            try
            {
                var remoteUrlWithRngParams = remoteUrl + $"&time={DateTime.Now.Ticks.GetHashCode()}";
                httpClient.BaseAddress = new Uri(remoteUrlWithRngParams);
                Debug.Log($"request to : {remoteUrlWithRngParams}");
                var result = await httpClient.GetAsync(remoteUrlWithRngParams);
                var resultContent = await result.Content.ReadAsStringAsync();
                var newContent = resultContent.Replace("\\\\", "\\");
                return newContent;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return await Task.FromResult(string.Empty);
            }
        }



        public async Task SaveDataAsync(Dictionary<SupportedLang, SystemLanguageData> resource)
        {
            if (!Directory.Exists(SaveDataPath))
            {
                Directory.CreateDirectory(SaveDataPath);
            }
            foreach (var lang in resource.Keys)
            {
                var langFolder = Path.Combine(SaveDataPath, lang.ToString().ToUpperInvariant());
                var filePath = Path.Combine(langFolder, $"{fileName}");
                if (!Directory.Exists(langFolder))
                {
                    Directory.CreateDirectory(langFolder);
                }
                var data = JsonConvert.SerializeObject(resource[lang].Datas, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, data);
                Debug.Log($"寫入 {filePath} 完成");
            }
        }

        private void ReplaceFile()
        {
            try
            {
                var langFolder = Path.Combine(SaveDataPath, targetLang);
                var filePath = Path.Combine(langFolder, $"{fileName}");
                File.Copy(filePath, targetSysLangPath, true);
                Debug.Log($"{fileName} 覆寫成功");
            }
            catch (Exception e)
            {
                Debug.LogError($"{fileName} 覆寫失敗");
                Debug.LogError(e.Message);
            }
        }
    }
}

#endif

