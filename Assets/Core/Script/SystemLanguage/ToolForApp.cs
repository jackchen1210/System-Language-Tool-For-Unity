
namespace Core.SystemLanguage
{
    public static class ToolForApp
    {
        public static string GetFTPFileNameBy(TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.Platform:
                    return SystemLanguageConfigData.PlatformFileFullName_FTP;
                case TargetType.Elect:
                    return SystemLanguageConfigData.ElectFileFullName_FTP;
                default:
                    return string.Empty;
            }
        }
    }
}
