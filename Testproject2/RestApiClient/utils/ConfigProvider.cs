using System;
using System.IO;
using RestApiClient.Models;

namespace RestApiClient.Utils
{
    public static class ConfigProvider
    {
        private const string ConfigFileName = "config.json";
        private const string ConfigDirectory = "resources";

        public static ConfigModel Load()
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, ConfigDirectory, ConfigFileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Конфигурационный файл не найден: {filePath}");
            }

            return JsonDataLoader.LoadFromJson<ConfigModel>(filePath);
        }
    }
}
