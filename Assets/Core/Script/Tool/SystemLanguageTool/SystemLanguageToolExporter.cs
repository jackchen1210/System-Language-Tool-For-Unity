
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.SystemLanguage
{
    public class SystemLanguageToolExporter
    {

        public enum ExportType
        {
            Code,
            Value
        }


        private ExportType exportType;
        private string customTargetSysLangPath;
        private string relativeSysLangPath = $@"Platform/TextAsset/{SystemLanguageConfigData.PlatformFileFullName}";
        private string TargetSysLangPath => Path.Combine(Application.dataPath, relativeSysLangPath);


        public SystemLanguageToolExporter SetTargetSysLangPath(string targetSysLangPath)
        {
            this.customTargetSysLangPath = targetSysLangPath;
            return this;
        }

        public SystemLanguageToolExporter SetExportType(ExportType exportType)
        {
            this.exportType = exportType;
            return this;
        }

        public async Task<string[]> ExportData()
        {
            try
            {
                var rawData = await File.ReadAllTextAsync(customTargetSysLangPath?? TargetSysLangPath);
                var datas = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawData);
                Debug.Log("匯出csv成功");
                if (exportType == ExportType.Code)
                {
                    return datas.Keys.ToArray();
                }
                else
                {

                    return datas.Values.ToArray();
                }
            }
            catch (Exception e)
            {
                Debug.Log("匯出csv失敗");
                Debug.LogError(e.Message);
                return new string[0];
            }
        }
    }
}
