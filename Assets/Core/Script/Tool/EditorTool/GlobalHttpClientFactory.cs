#if UNITY_EDITOR

using System.Net.Http;
namespace Core.EditorTool
{
    public static class GlobalHttpClientFactory
    {
        private static HttpClient httpClient = new HttpClient();
        public static HttpClient Default => httpClient;
        public static HttpClient Create()
        {
            return new HttpClient();
        }
    }

}
#endif
