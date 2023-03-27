using System.IO;
using Core.EditorTool;
using UnityEngine;

namespace Core.SystemLanguage
{
    public static class SystemLanguageToolFactory
    {
        public static SystemLanguageTool CreateWithHttpClient(TargetType targetType)
        {
            return new SystemLanguageTool(GlobalHttpClientFactory.Create())
                .SetRemoteUrl(Tool.GetRemoteUrlBy(targetType))
                .SetTargetSysLangPath(Tool.GetTargetSysLangPathBy(targetType))
                .SetFileName(Tool.GetFileNameBy(targetType));
        }


    }
}
