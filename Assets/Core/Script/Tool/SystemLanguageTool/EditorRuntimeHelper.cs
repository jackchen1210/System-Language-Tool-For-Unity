using System;
using System.Threading.Tasks;
using Core;
using Core.SystemLanguage;
using UnityEditor;
using UnityEngine;
public class EditorRuntimeHelper
{
    private static Action onEnd;

    [RuntimeInitializeOnLoadMethod]
    private static void RunOnSceneLoad()
    {
       // SystemKeyLibraryStepEvent.OnDoEditorStep -= OnDoEditorStep;
       // SystemKeyLibraryStepEvent.OnDoEditorStep += OnDoEditorStep;
    }

    private static async void OnDoEditorStep(Action onEnd)
    {
        EditorRuntimeHelper.onEnd = onEnd;
        await RequestData();
    }

    private static async Task RequestData()
    {
        Debug.Log("[Editor]更新本地文字檔資源");
        var systemLanguageTool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);
        await systemLanguageTool.RunAsync();
        systemLanguageTool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect);
        await systemLanguageTool.RunAsync();
        AssetDatabase.Refresh();
        onEnd?.Invoke();
    }

}
