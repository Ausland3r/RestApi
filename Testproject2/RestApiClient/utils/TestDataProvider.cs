using System.IO;
using NUnit.Framework;
using RestApiClient.Models;

namespace RestApiClient.Utils
{
    public static class TestDataProvider
    {
        private const string TestDataFileName = "testdata.json";
        private const string TestDatagDirectory = "resources";
        public static TestData Load()
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, TestDatagDirectory, TestDataFileName);
            return JsonDataLoader.LoadFromJson<TestData>(filePath);
        }
    }
}
