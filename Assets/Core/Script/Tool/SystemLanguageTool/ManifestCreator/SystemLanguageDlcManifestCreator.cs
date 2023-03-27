
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.SystemLanguage
{
    public class SystemLanguageDlcManifestCreator
    {
        private Dictionary<TargetType, string> cacheData = new Dictionary<TargetType, string>();

        public async  Task<SystemLanguageDlcManifest> PullDataFromRemoteAndCreateManifestAsync()
        {
            var output = new SystemLanguageDlcManifest();
            foreach (var targetType in Enum.GetValues(typeof(TargetType))
                .Cast<TargetType>()
                .ToList())
            {
                foreach (var supportedLang in Enum.GetValues(typeof(SupportedLang))
                     .Cast<SupportedLang>()
                     .ToList())
                {
                    var md5 = await CreateMD5Async(targetType, supportedLang);
                    Debug.Log("MD5 created \n " +
                        $"TargetType : {targetType}\n" +
                        $"SupportedLang : {supportedLang}\n" +
                        $"MD5 : {md5}");
                    output.Datas.Add(new SystemLanguageDlcData
                    {
                        SupportedLang = supportedLang,
                        TargetType = targetType,
                        MD5 = md5
                    });
                }
            }

            return output;
        }

        public async Task<string> CreateMD5Async(TargetType platform, SupportedLang supportedLang)
        {
            if (!cacheData.ContainsKey(platform))
            {
                var tool = SystemLanguageToolFactory.CreateWithHttpClient(platform);
                var data = await tool.PullDataAsync();
                cacheData.Add(platform, data);
            }
            var rawData = Tool.ParseRawDataFromCsv(cacheData[platform])[supportedLang].Datas;
            var _data = JsonConvert.SerializeObject(rawData);
            var output = MD5Tool.Compute(_data);
            return output;
        }

        public async Task PullDataFromRemoteAndCreateManifestSaveLocalAsync(string localPath)
        {
            var data = await PullDataFromRemoteAndCreateManifestAsync();
            await File.WriteAllTextAsync(localPath,JsonConvert.SerializeObject(data, Formatting.Indented));
        }
    }
}
