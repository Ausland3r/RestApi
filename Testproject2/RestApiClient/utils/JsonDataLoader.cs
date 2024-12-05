using System;
using System.IO;
using Newtonsoft.Json;

namespace RestApiClient.Utils
{
    public static class JsonDataLoader
    {
        public static T LoadFromJson<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"JSON file not found at {filePath}");
            }

            string jsonContent = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<T>(jsonContent)
                   ?? throw new InvalidDataException($"Deserialization returned null for type {typeof(T).Name}.");
        }
    }
}
