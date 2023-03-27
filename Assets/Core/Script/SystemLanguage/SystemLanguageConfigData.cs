using System.IO;
using UnityEngine;

namespace Core.SystemLanguage
{
    public class SystemLanguageConfigData
    {
        public static string SaveDataPath => Path.Combine(Application.dataPath, LocalLangPath);
        public static string FtpLangFolderName => "Lang";
        public static string LocalLangPath => "Platform/TextAsset/Data/Lang";
        public static string DlcManifestFullPath => Path.Combine(SaveDataPath, DlcManifestFullName);
        public static string DlcManifestFullName => "DlcManifest.bytes";
        public static string ElectFileFullName => "LangageStringList.bytes";
        public static string PlatformFileFullName => "LangageStringList_Platform.bytes";
        public static string DlcManifestFullName_FTP => "DlcManifest.eg";
        public static string ElectFileFullName_FTP => "LangageStringList.eg";
        public static string PlatformFileFullName_FTP => "LangageStringList_Platform.eg";
        public static SupportedLang CurrentLang { get; } = SupportedLang.ZH_TW;
        public static string TableId_platform => "0";
        public static string TableId_elect => "762715079";
        public static string GoogleSheetRemoteUrl => @"GOOGLE_SHEET_URL_PATH";
        public static string SheetId => "SHEET_ID";

        public static string GetRemoteUrlBy(TargetType targetType)
        {
            var tableId = string.Empty;
            switch (targetType)
            {
                case TargetType.Platform:
                    tableId = TableId_platform;
                    break;
                case TargetType.Elect:
                    tableId = TableId_elect;
                    break;
            }
            return string.Format(GoogleSheetRemoteUrl, SheetId, tableId);
        }
    }
}
