using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RestApiClient.Models;
using RestApiClient.Utils;

namespace RestApiClient.Tests
{
    public class BaseTest
    {
        protected ApiUtils ApiUtils = null!;
        protected TestData TestData = null!;
        protected ConfigModel ConfigData = null!;
        protected ILogger<BaseTest> Logger = null!;

        [SetUp]
        public void SetUp()
        {
            TestData = TestDataProvider.Load();
            ConfigData = ConfigProvider.Load();
            ApiUtils = new ApiUtils(ConfigData.BaseUrl);

            Logger = LoggerProvider.CreateLogger<BaseTest>();
            Logger.LogInformation("Данные загружены: BaseUrl={BaseUrl}, Endpoints={Endpoints}", 
            ConfigData.BaseUrl, ConfigData.Endpoints);

        }
    }
}
