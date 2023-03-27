#if UNITY_EDITOR

using UnityEngine;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Linq;

namespace Core.SystemLanguage.UniTest
{
    public class SystemLanguageFtpToolTest
    {
        private SystemLanguageFtpManager ftpManager;

        //[Test]
        //public void CheckIsVpnConnect()
        //{
        //    if (!NetworkInterface.GetIsNetworkAvailable())
        //    {
        //        Debug.LogError("No network available");
        //        throw new System.Exception("需開啟ELE VPN");
        //    }
        //    NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        //    var names = interfaces.Where(_ => _.OperationalStatus == OperationalStatus.Up)
        //         .Where(_ => _.NetworkInterfaceType == NetworkInterfaceType.Ppp && (_.NetworkInterfaceType != NetworkInterfaceType.Loopback));
        //    var isUsingVpn = names
        //         .Where(_ => _.Name.Contains("0.alltumi.com"))
        //         .Count() > 0;
        //    Debug.Log($"Current network names : {JsonConvert.SerializeObject(names.Where(_ => _ != null).Select(_ => _.Name).ToArray(), Formatting.Indented)}");
        //    Debug.Log($"Current network description : {JsonConvert.SerializeObject(names.Where(_ => _ != null).Select(_ => _.Description).ToArray(), Formatting.Indented)}");

        //    if (!isUsingVpn)
        //    {
        //        throw new System.Exception("需開啟ELE VPN");
        //    }
        //}

        [Test]
        public void ReadFtpConfigDataSuccess()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            Debug.Log(JsonConvert.SerializeObject(ftpTool.ConfigData, Formatting.Indented));
            Assert.IsNotNull(ftpTool.ConfigData);
        }

        private SystemLanguageFtpManager GetSystemLanguageFtpManager()
        {
            if(ftpManager == null)
            {
                ftpManager = SystemLanguageFtpManager.GetInstance();
            }
            return ftpManager;
        }


        [Test]
        public void Check_RD1_Zhtw_Lang_Directory_Exist()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            Assert.True(ftpTool.CreateFTPDirectory(SystemLanguageFtpManager.HostType.RD1, SupportedLang.ZH_TW));
        }
        [Test]
        public async Task Upload_Platform_Zhtw_Data_To_RD1_Success_Async()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            await ftpTool.CreateDirectoryAndUploadFileAsync(SystemLanguageFtpManager.HostType.RD1, SupportedLang.ZH_TW, TargetType.Platform);
        }
        [Test]
        public async Task Upload_Elect_Zhtw_Data_To_RD1_Success_Async()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            await ftpTool.CreateDirectoryAndUploadFileAsync(SystemLanguageFtpManager.HostType.RD1, SupportedLang.ZH_TW, TargetType.Elect);
        }
        [Test]
        public async Task Upload_Platform_EnUS_Data_To_RD1_Success_Async()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            await ftpTool.CreateDirectoryAndUploadFileAsync(SystemLanguageFtpManager.HostType.RD1, SupportedLang.EN_US, TargetType.Platform);
        }
        [Test]
        public async Task Upload_Elect_EnUS_Data_To_RD1_Success_Async()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            await ftpTool.CreateDirectoryAndUploadFileAsync(SystemLanguageFtpManager.HostType.RD1, SupportedLang.EN_US, TargetType.Elect);
        }
        [Test]
        public async Task Upload_Platform_Zhtw_Data_To_RD2_Success_Async()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            await ftpTool.CreateDirectoryAndUploadFileAsync(SystemLanguageFtpManager.HostType.RD2, SupportedLang.ZH_TW, TargetType.Platform);
        }
        [Test]
        public async Task Upload_Elect_Zhtw_Data_To_RD2_Success_Async()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            await ftpTool.CreateDirectoryAndUploadFileAsync(SystemLanguageFtpManager.HostType.RD2, SupportedLang.ZH_TW, TargetType.Elect);
        }
        [Test]
        public async Task Upload_Platform_Zhtw_Data_To_RD3_Success_Async()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            await ftpTool.CreateDirectoryAndUploadFileAsync(SystemLanguageFtpManager.HostType.RD3, SupportedLang.ZH_TW, TargetType.Platform);
        }
        [Test]
        public async Task Upload_Elect_Zhtw_Data_To_RD3_Success_Async()
        {
            var ftpTool = GetSystemLanguageFtpManager();
            await ftpTool.CreateDirectoryAndUploadFileAsync(SystemLanguageFtpManager.HostType.RD3, SupportedLang.ZH_TW, TargetType.Elect);
        }
    }
}

#endif