using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Core.SystemLanguage.UniTest
{
    public class SystemLanguageToolTest
    {
        [Test]
        public async Task Successfully_Pull_Platform_Data_From_Google_Sheet_Async()
        {

            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);
            var data = await tool.PullDataAsync();
            Debug.Log(data);
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }
        [Test]
        public async Task Successfully_Pull_Elect_Data_From_Google_Sheet_Async()
        {

            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect);
            var data = await tool.PullDataAsync();
            Debug.Log(data);
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }
        [Test]
        public async Task Successfully_Pull_Platform_Data_From_Google_Sheet_Created_By_Factory_Async()
        {
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);
            var data = await tool.PullDataAsync();
            Debug.Log(data);
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }
        [Test]
        public async Task Successfully_Pull_Elect_Data_From_Google_Sheet_Created_By_Factory_Async()
        {
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect);
            var data = await tool.PullDataAsync();
            Debug.Log(data);
            Assert.IsFalse(string.IsNullOrEmpty(data));
        }

        [Test]
        public async Task Successfully_Pull_Platform_Data_From_Google_Sheet_And_Correct_Async()
        {
            var firstLineText = @"<height=388><padding=60><spacing= 30><alignment=left>※ 密碼限7~20個英數字符號 \\n   (不可輸入空格) \\n※ 密碼需包含大寫字母、小寫字母， \\n   及一個以上的數字".Replace("\\\\", "\\");

            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);
            var data = await tool.PullDataAsync();
            Debug.Log(data);
            Assert.IsTrue(data.Contains(firstLineText));
        }
        [Test]
        public async Task Successfully_Pull_Elect_Data_From_Google_Sheet_And_Correct_Async()
        {
            var firstLineText = "網路連線異常，請重新下載[$]";

            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect);
            var data = await tool.PullDataAsync();
            Debug.Log(data);
            Assert.IsTrue(data.Contains(firstLineText));
        }
        [Test]
        public async Task Successfully_Get_Platform_Data_Title_Using_CsvHelper_Async()
        {
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);

            var data = await tool.PullDataAsync();
            var titles = Tool.GetTitleFromRawData(data);
            Debug.Log(JsonConvert.SerializeObject(titles));
            Assert.AreEqual(titles[0], "Code");
            Assert.AreEqual(titles[1], "zh-TW");
            Assert.AreEqual(titles[2], "en-US");
        }
        [Test]
        public async Task Successfully_Get_Elect_Data_Title_Using_CsvHelper_Async()
        {
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect);

            var data = await tool.PullDataAsync();
            var titles = Tool.GetTitleFromRawData(data);
            Debug.Log(JsonConvert.SerializeObject(titles));
            Assert.AreEqual(titles[0], "Code");
            Assert.AreEqual(titles[1], "zh-TW");
            Assert.AreEqual(titles[2], "en-US");
        }

        [Test]
        public async Task Successfully_Get_Zhtw_Language_Platform_Data_Using_CsvHelper_Async()
        {
            var firstLineText = @"<height=388><padding=60><spacing= 30><alignment=left>※ 密碼限7~20個英數字符號 \\n   (不可輸入空格) \\n※ 密碼需包含大寫字母、小寫字母， \\n   及一個以上的數字".Replace("\\\\", "\\");
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);

            var data = await tool.PullDataAsync();
            var rawDatas = Tool.ParseRawDataFromCsv(data);
            Debug.Log(JsonConvert.SerializeObject(rawDatas));
            Assert.AreEqual(firstLineText, rawDatas[SupportedLang.ZH_TW].Datas["PASSWORD_RULES"]);
        }
        [Test]
        public async Task Successfully_Get_Zhtw_Language_Elect_Data_Using_CsvHelper_Async()
        {
            var firstLineText = "網路連線異常，請重新下載[$]";
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect);

            var data = await tool.PullDataAsync();
            var rawDatas = Tool.ParseRawDataFromCsv(data);
            Debug.Log(JsonConvert.SerializeObject(rawDatas));
            Assert.AreEqual(firstLineText, rawDatas[SupportedLang.ZH_TW].Datas["NET_ERROR_006"]);
        }
        [Test]
        public async Task Successfully_Get_EnUS_Language_Platform_Data_Using_CsvHelper_Async()
        {
            var firstLineText = "test1";
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform).SetCurrentLanguage(SupportedLang.EN_US);

            var data = await tool.PullDataAsync();
            var rawDatas = Tool.ParseRawDataFromCsv(data);
            Debug.Log(JsonConvert.SerializeObject(rawDatas, Formatting.Indented));
            Assert.AreEqual(firstLineText, rawDatas[SupportedLang.EN_US].Datas["PASSWORD_RULES"]);
        }
        [Test]
        public async Task Successfully_Get_EnUS_Language_Elect_Data_Using_CsvHelper_Async()
        {
            var firstLineText = "test2";
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect).SetCurrentLanguage(SupportedLang.EN_US);

            var data = await tool.PullDataAsync();
            var rawDatas = Tool.ParseRawDataFromCsv(data);
            Debug.Log(JsonConvert.SerializeObject(rawDatas, Formatting.Indented));
            Assert.AreEqual(firstLineText, rawDatas[SupportedLang.EN_US].Datas["NET_ERROR_006"]);
        }

        [Test]
        public async Task Successfully_Save_Platform_Language_Data_Async()
        {
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Platform);
            await tool.RunAsync();
        }
        [Test]
        public async Task Successfully_Save_Elect_Language_Data_Async()
        {
            var tool = SystemLanguageToolFactory.CreateWithHttpClient(TargetType.Elect);
            await tool.RunAsync();
        }
    }

}
