using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using static Core.SystemLanguage.SystemLanguageToolExporter;

namespace Core.SystemLanguage.UniTest
{
    public class SystemLanguageToolExporterTest
    {
        private string PlatformDataPath => Path.Combine(Application.dataPath, @"Platform/TextAsset/LangageStringList_Platform.bytes");
        private string ElecDataPath => Path.Combine(Application.dataPath, @"EGames/EGCore/TextData/LangageStringList.bytes");

        private SystemLanguageToolExporter CreateExporter(ExportType exportType)
        {
            return new SystemLanguageToolExporter().SetExportType(exportType);
        }

        [Test]
        public async Task ExportCodesFromPlatformDataAsync()
        {
            var data = await CreateExporter( ExportType.Code).SetTargetSysLangPath(PlatformDataPath).ExportData();

            Debug.Log(data.Aggregate((_, __) => _ += $"\n{__}"));
            Assert.IsTrue(data.Length > 0);
        }

        [Test]
        public async Task ExportValuesFromPlatformDataAsync()
        {
            var data = await CreateExporter(ExportType.Value).SetTargetSysLangPath(PlatformDataPath).ExportData();

            Debug.Log(data.Aggregate((_, __) => _ += $"\n{__}"));
            Assert.IsTrue(data.Length > 0);
        }


        [Test]
        public async Task ExportCodesFromElecDataAsync()
        {
            var data = await CreateExporter(ExportType.Code).SetTargetSysLangPath(ElecDataPath).ExportData();

            Debug.Log(data.Aggregate((_, __) => _ += $"\n{__}"));
            Assert.IsTrue(data.Length > 0);
        }

        [Test]
        public async Task ExportValuesFromElecDataAsync()
        {
            var data = await CreateExporter(ExportType.Value).SetTargetSysLangPath(ElecDataPath).ExportData();

            Debug.Log(data.Aggregate((_, __) => _ += $"\n{__}"));
            Assert.IsTrue(data.Length > 0);
        }
    }
}
