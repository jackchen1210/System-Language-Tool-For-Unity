#if UNITY_EDITOR

using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Core.SystemLanguage.UniTest
{

    public class SystemLanguageDlcManifestCreatorTest
    {

        [Test]
        public async Task PullData_And_Calculate_Platform_zhTW_Language_Data_MD5_Async()
        {
            var creator = new SystemLanguageDlcManifestCreator();
            var md5 = await creator.CreateMD5Async(TargetType.Platform, SupportedLang.ZH_TW);
            Debug.Log(md5);
            Assert.IsNotNull(md5);
        }

        [Test]
        public async Task Pull_Data_From_Remote_And_Create_Zhtw_Manifest_Success_Async()
        {
            var creator = new SystemLanguageDlcManifestCreator();
            var maniFest = await creator.PullDataFromRemoteAndCreateManifestAsync();
            var stringifyManifest = JsonConvert.SerializeObject(maniFest, Formatting.Indented);
            Debug.Log(stringifyManifest);
            Assert.IsNotNull(maniFest.Datas.FirstOrDefault(_ => _.SupportedLang == SupportedLang.ZH_TW));
        }
        [Test]
        public async Task Pull_Data_From_Remote_And_Create_Manifest_And_Push_To_RD1_Ftp_Success_Async()
        {
            var creator = new SystemLanguageDlcManifestCreator();
            await creator.PullDataFromRemoteAndCreateManifestSaveLocalAsync(SystemLanguageConfigData.DlcManifestFullPath);
            await SystemLanguageFtpManager.GetInstance().CreateDirectoryAndUploadManifestAsync(SystemLanguageFtpManager.HostType.RD1, SystemLanguageConfigData.DlcManifestFullPath);
        }
        [Test]
        public async Task Pull_Manifest_From_RD1_FTP_Async()
        {
            var dlcUrl = SystemLanguageFtpManager.GetInstance().ConfigData.First(_ => string.Equals(_.Host, SystemLanguageFtpManager.HostType.RD1.ToString()))._DomainPathList;
            var data = await new SystemLanguageToolForApp().GetManifestAsync(dlcUrl); 
            Debug.Log(JsonConvert.SerializeObject(data, Formatting.Indented));
            Assert.IsNotNull(data);
        }

        [Test]
        public async Task Pull_zhtw_Platform_From_RD1_FTP_Async()
        {
            var dlcUrl = SystemLanguageFtpManager.GetInstance().ConfigData.First(_ => string.Equals(_.Host, SystemLanguageFtpManager.HostType.RD1.ToString()))._DomainPathList;
            var data = await new SystemLanguageToolForApp().GetFileAsync(dlcUrl, TargetType.Platform, SupportedLang.ZH_TW);
            Debug.Log(JsonConvert.SerializeObject(data, Formatting.Indented));
            Assert.IsNotNull(data);
        }
        [Test]
        public async Task Pull_zhtw_Elect_From_RD1_FTP_Async()
        {
            var dlcUrl = SystemLanguageFtpManager.GetInstance().ConfigData.First(_ => string.Equals(_.Host, SystemLanguageFtpManager.HostType.RD1.ToString()))._DomainPathList;
            var data = await new SystemLanguageToolForApp().GetFileAsync(dlcUrl, TargetType.Elect, SupportedLang.ZH_TW);
            Debug.Log(JsonConvert.SerializeObject(data, Formatting.Indented));
            Assert.IsNotNull(data);
        }
        [Test]
        public async Task Pull_enUS_Elect_From_RD1_FTP_Async()
        {
            var dlcUrl = SystemLanguageFtpManager.GetInstance().ConfigData.First(_ => string.Equals(_.Host, SystemLanguageFtpManager.HostType.RD1.ToString()))._DomainPathList;
            var data = await new SystemLanguageToolForApp().GetFileAsync(dlcUrl, TargetType.Elect, SupportedLang.EN_US);
            Debug.Log(JsonConvert.SerializeObject(data, Formatting.Indented));
            Assert.IsNotNull(data);
        }
        [Test]
        public async Task Pull_enUS_Platform_From_RD1_FTP_Async()
        {
            var dlcUrl = SystemLanguageFtpManager.GetInstance().ConfigData.First(_ => string.Equals(_.Host, SystemLanguageFtpManager.HostType.RD1.ToString()))._DomainPathList;
            var data = await new SystemLanguageToolForApp().GetFileAsync(dlcUrl, TargetType.Platform, SupportedLang.EN_US);
            Debug.Log(JsonConvert.SerializeObject(data, Formatting.Indented));
            Assert.IsNotNull(data);
        }
    }

}
#endif