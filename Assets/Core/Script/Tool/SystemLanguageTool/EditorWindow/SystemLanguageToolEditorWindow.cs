using System.IO;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
namespace Core.SystemLanguage
{
    public class SystemLanguageToolEditorWindow : OdinEditorWindow
    {
        private bool isLock;

        [MenuItem("自定義編輯器/多國語系載入器", priority = 1003)]
        private static void OpenStringListEditorWindow()
        {
            GetWindow<SystemLanguageToolEditorWindow>().Show();
        }
        private bool isLockRd;

        [Button("下載並更新本地端")]
        public async void DownloadAndUpdate()
        {
            if (isLock)
            {
                Debug.LogError("處理中，請稍後");
                return;
            }
            isLock = true;
            var systemLanguageTool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);
            await systemLanguageTool.RunAsync();
            systemLanguageTool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect); 
            await systemLanguageTool.RunAsync();
            AssetDatabase.Refresh();
            isLock = false;
        }
        [Button("下載並更新RD1"),FoldoutGroup("更新FTP")]
        public async void DownloadAndUploadRd1()
        {
            await DownloadAndUpload(SystemLanguageFtpManager.HostType.RD1);
        }
        [Button("下載並更新RD2"), FoldoutGroup("更新FTP")]
        public async void DownloadAndUploadRd2()
        {
            await DownloadAndUpload(SystemLanguageFtpManager.HostType.RD2);
        }
        [Button("下載並更新RD3"), FoldoutGroup("更新FTP")]
        public async void DownloadAndUploadRd3()
        {
            await DownloadAndUpload(SystemLanguageFtpManager.HostType.RD3);
        }

        private async Task DownloadAndUpload(SystemLanguageFtpManager.HostType hostType)
        {
            if (isLockRd)
            {
                Debug.LogError("處理中，請稍後");
                return;
            }
            isLockRd = true;
            var systemLanguageTool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);
            await systemLanguageTool.RunAsync();
            systemLanguageTool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect);
            await systemLanguageTool.RunAsync();
            var ftpManager = SystemLanguageFtpManager.GetInstance();
            await ftpManager.UploadLangFolderAsync(hostType);
            isLockRd = false;
        }
        [Button("指向平台設定檔"), FoldoutGroup("搜尋")]
        public void PingPlatformLangData()
        {
            var path = Path.Combine("Assets", Tool.GetTargetSysLangPathBy(TargetType.Platform, true));
            EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(path));
        }

        [Button("指向電子館設定檔"), FoldoutGroup("搜尋")]
        public void PingElectLangData()
        {
            var path =Path.Combine("Assets",Tool.GetTargetSysLangPathBy(TargetType.Elect, true));
            var go = AssetDatabase.LoadMainAssetAtPath(path);
            EditorGUIUtility.PingObject(go);
        }
    }

}