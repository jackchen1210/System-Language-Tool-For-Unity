using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Core.SystemLanguage
{
    public static class MD5Tool
    {
        public static string Compute(string data)
        {
            var md5 = MD5.Create();
            var md5Data = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            return md5Data.Select(_ => _.ToString("x2")).Aggregate((_, __) => _ + __);
        }
    }
}
