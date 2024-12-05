using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace RestApiClient.Utils
{
    public class ApiUtils
    {
        private readonly RestClient _client;
        private readonly ILogger<ApiUtils> _logger;

        public ApiUtils(string baseUrl)
        {
            _client = new RestClient(baseUrl);
            _logger = LoggerProvider.CreateLogger<ApiUtils>();  
        }

    
    public async Task<RestResponse<T>> GetAsync<T>(string url)
    {
        _logger.LogInformation("Отправка GET запроса на {Url}", url);

        var request = new RestRequest(url, Method.Get);
        var response = await _client.ExecuteAsync<T>(request);


        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogWarning("GET запрос на {Url} вернул 404 Not Found", url);
            return new RestResponse<T>(request)
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Data = default
            };
        }

        if (!response.IsSuccessful)
        {
            _logger.LogError("Ошибка при выполнении GET запроса на {Url}: {StatusCode} - {ErrorMessage}",
            url, response.StatusCode, response.ErrorMessage);
            throw new HttpRequestException($"Ошибка: {response.StatusCode} - {response.ErrorMessage}");
        }
        
        _logger.LogInformation("Успешный GET запрос на {Url}. Код ответа: {StatusCode}", url, response.StatusCode);
        return response;
    }

        public async Task<RestResponse<T>> PostAsync<T>(string url, object payload)
        {
            _logger.LogInformation("Отправка POST запроса на {Url} с данными: {Payload}", url, JsonSerializer.Serialize(payload));
            var request = new RestRequest(url, Method.Post);
            request.AddJsonBody(payload);
            var response = await _client.ExecuteAsync<T>(request);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Ошибка при выполнении POST запроса на {Url}: {StatusCode} - {ErrorMessage}",
                url, response.StatusCode, response.ErrorMessage);
                throw new HttpRequestException($"Ошибка: {response.StatusCode} - {response.ErrorMessage}");
            }

            _logger.LogInformation("Успешный POST запрос на {Url}. Код ответа: {StatusCode}", url, response.StatusCode);
            return response;
        }
    }

}
