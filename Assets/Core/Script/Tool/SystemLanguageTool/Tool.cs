
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;

namespace Core.SystemLanguage
{
    public static class Tool
    {
        public static string GetFileNameBy(TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.Platform:
                    return SystemLanguageConfigData.PlatformFileFullName;
                case TargetType.Elect:
                    return SystemLanguageConfigData.ElectFileFullName;
                default:
                    return string.Empty;
            }
        }
        public static string GetFTPFileNameBy(TargetType targetType)
        {
            return ToolForApp.GetFTPFileNameBy(targetType);
        }
        public static string GetRemoteUrlBy(TargetType targetType)
        {
            return SystemLanguageConfigData.GetRemoteUrlBy(targetType);
        }        
        /// <summary>
        /// key : lang type
        /// value : data
        /// </summary>
        /// <returns></returns>
        public static Dictionary<SupportedLang, SystemLanguageData> ParseRawDataFromCsv(string data)
        {
            var titles = GetTitleFromRawData(data);
            //skip 1 是為了不要"code"這個標題
            var output = titles.Skip(1).Select(_ => (SupportedLang)Enum.Parse(typeof(SupportedLang), _.Replace("-", "_").ToUpperInvariant())).ToDictionary(_ => _, _ => new SystemLanguageData());

            using (var stringReader = new StringReader(data))
            using (var csvReader = new CsvReader(stringReader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<SystemLanguageRecord>();
                var langNames = SystemLanguageRecord.Empty.GetType().GetProperties().Select(_ => _.Name.ToUpperInvariant()).Where(_ => !string.Equals(_, nameof(SystemLanguageRecord.Empty.Code), StringComparison.InvariantCultureIgnoreCase));

                foreach (var line in records)
                {
                    foreach (var lang in langNames)
                    {
                        if (output.TryGetValue((SupportedLang)Enum.Parse(typeof(SupportedLang), lang), out var systemLanguageData))
                        {
                            var addValue = (string)line.GetType().GetProperty(lang).GetValue(line);
                            if (!systemLanguageData.Datas.TryAdd(line.Code, addValue))
                            {
                                Debug.LogError($"Add value failed \n" +
                                                $"Code : {line.Code} \n" +
                                                $"addValue : {addValue}");
                            }
                        }
                        else
                        {
                            Debug.LogError($"{lang} not found.");
                        }
                    }
                }
            }
            return output;
        }
        public static string[] GetTitleFromRawData(string data)
        {
            using (var stringReader = new StringReader(data))
            using (var csvReader = new CsvReader(stringReader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                return csvReader.HeaderRecord;
            }
        }
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            var dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"找不到目標資料夾 : {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo file in dir.GetFiles().Where(_ => !_.Name.Contains("meta")))
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name.Replace(".bytes", ".eg"));
                file.CopyTo(targetFilePath, true);
                Debug.Log($"複製到 : {targetFilePath}");
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
        public static string GetTargetSysLangPathBy(TargetType targetType,bool relative = false)
        {
            var platformRelative = @$"Platform/TextAsset/{Tool.GetFileNameBy(targetType)}";
            var electRelative = @$"EGames/EGCore/TextData/{Tool.GetFileNameBy(targetType)}";
            if (relative)
            {
                switch (targetType)
                {
                    case TargetType.Platform:
                        return platformRelative;
                    case TargetType.Elect:
                        return electRelative;
                    default:
                        return string.Empty;
                }
            }

            switch (targetType)
            {
                case TargetType.Platform:
                    return Path.Combine(Application.dataPath, platformRelative);
                case TargetType.Elect:
                    return Path.Combine(Application.dataPath, electRelative);
                default:
                    return string.Empty;
            }
        }
    }
}
