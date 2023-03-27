#if UNITY_EDITOR

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Core.SystemLanguage
{
    public class SystemLanguageFtpManager
    {
        public enum HostType
        {
            RD1,
            RD2,
            RD3,
            QA
        }
        public class FTPInfo
        {
            public string Host;
            public List<DLCFTPInfo> DLCInfo;
            public List<APPFTPInfo> APPInfo;
            public string _DomainPathList;
            public bool Enable;
        }
        public class APPFTPInfo
        {
            public string APPUrl;
            public bool UsePassive;
            public string APPUserName;
            public string APPPassword;
        }
        public class DLCFTPInfo
        {
            public string DLCUrl;
            public bool UsePassive;
            public string DLCUserName;
            public string DLCPassword;
        }


        private static SystemLanguageFtpManager instance;

        private TextAsset ftpJson;
        public List<FTPInfo> ConfigData { get; private set; }


        private SystemLanguageFtpManager()
        {
            GetFTPJSON();
            string data = GetFTPJSON()?.text;
            ConfigData = JsonConvert.DeserializeObject<List<FTPInfo>>(data);
        }
        public static SystemLanguageFtpManager GetInstance()
        {
            if (instance == null)
            {
                instance = new SystemLanguageFtpManager();
            }
            return instance;
        }

        private TextAsset GetFTPJSON()
        {
            //指定預設JSON檔
            if (ftpJson == null)
            {
                ftpJson = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/EGames/EGCore/TextData/FTPInfo.bytes");
            }
            return ftpJson;
        }

        public async Task CreateDirectoryAndUploadFileAsync(HostType hostType, SupportedLang supportedLang, TargetType targetType)
        {
            Debug.Log($"CreateDirectoryAndUploadFileAsync : {hostType} {supportedLang}");
            CreateFTPDirectory(hostType, supportedLang);

            var exceptions = new ConcurrentQueue<Exception>();

            var dlcInfos = ConfigData.First(_ => string.Equals(_.Host, hostType.ToString())).DLCInfo;
            foreach (var dlcInfo in dlcInfos)
            {
                try
                {
                    var ftpTool = new FTPTool(dlcInfo);
                    await ftpTool.UploadFileAsync(supportedLang, targetType);
                }
                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                }
            }

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }
        }

        public bool CreateFTPDirectory(HostType hostType, SupportedLang supportedLang)
        {
            var dlcInfos = ConfigData.First(_ => string.Equals(_.Host, hostType.ToString())).DLCInfo.First();
            var ftp = new FTPTool(dlcInfos);
            return ftp.CreateFTPDirectory(supportedLang);
        }

        public async Task CreateDirectoryAndUploadManifestAsync(HostType hostType,string filePath)
        {
            CreateFTPDirectory(hostType);

            var exceptions = new ConcurrentQueue<Exception>();

            var dlcInfos = ConfigData.First(_ => string.Equals(_.Host, hostType.ToString())).DLCInfo;
            foreach (var dlcInfo in dlcInfos)
            {
                try
                {
                    var ftpTool = new FTPTool(dlcInfo);
                    await ftpTool.UploadManifestFileAsync(filePath);
                }
                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                }
            }

            if (!exceptions.IsEmpty)
            {
                throw new AggregateException(exceptions);
            }
        }
        private bool CreateFTPDirectory(HostType hostType)
        {
            var dlcInfos = ConfigData.First(_ => string.Equals(_.Host, hostType.ToString())).DLCInfo.First();
            var ftp = new FTPTool(dlcInfos);
            return ftp.CreateFTPDirectory();
        }

        public async Task UploadLangFolderAsync(HostType hostType)
        {
            var dlcInfos = ConfigData.First(_ => string.Equals(_.Host, hostType.ToString())).DLCInfo.First();
            var ftp = new FTPTool(dlcInfos);
            await ftp.UploadFolderAsync(SystemLanguageConfigData.SaveDataPath,"meta");
        }
    }
}

#endif