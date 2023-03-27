
using CsvHelper.Configuration.Attributes;

namespace Core.SystemLanguage
{
    public class SystemLanguageRecord
    {
        public static SystemLanguageRecord Empty = new SystemLanguageRecord();

        public string Code { get; set; }
        [Name("zh-TW")]
        public string ZH_TW { get; set; }
        [Name("en-US")]
        public string EN_US { get; set; }
    }
}
