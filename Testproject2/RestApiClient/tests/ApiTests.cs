using NUnit.Framework;
using RestApiClient.Models;
using RestApiClient.Utils;
using Microsoft.Extensions.Logging;

namespace RestApiClient.Tests
{
    public class ApiTests : BaseTest
    {
        private readonly ILogger<ApiTests> _logger;

        public ApiTests()
        {
            _logger = LoggerProvider.CreateLogger<ApiTests>();
        }

        [Test]
        public async Task TestPostsApi()
        {
            var postsResponse = await ApiUtils.GetAsync<List<Post>>(ConfigData.Endpoints.Posts);
            _logger.LogInformation("Получено {Count} пользователей.", postsResponse.Data?.Count ?? 0);
            Assert.Multiple(() =>
            {
                Assert.That(postsResponse.Data, Is.Not.Null.And.Not.Empty, "Список не должен быть null");
                Assert.That(postsResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Код ответа должен быть 200.");
                Assert.That(postsResponse.Data, Is.Ordered.Ascending.By(nameof(Post.Id)), "Сообщения должны быть отсортированы по ID.");
            });
            _logger.LogInformation("Проверка списка пользователей завершена успешно.");

            var specificPostId = Environment.GetEnvironmentVariable("SPECIFIC_POST_ID");
            _logger.LogInformation("Запрос конкретного пользователя с ID {Id}.", specificPostId);
            var specificPostResponse = await ApiUtils.GetAsync<Post>($"{ConfigData.Endpoints.Posts}/{specificPostId}");
            _logger.LogInformation("Получен пользователь с ID: {Id}.", specificPostResponse.Data?.Id);
            Assert.Multiple(() =>
            {
                Assert.That(specificPostResponse, Is.Not.Null, "Ответ API не должен быть null.");
                Assert.That(specificPostResponse.Data, Is.Not.Null, "Сообщение не должно быть null.");
                Assert.That(specificPostResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Код ответа должен быть 200.");
                Assert.That(specificPostResponse.Data?.Id, Is.EqualTo(TestData.SpecificPostId), "ID сообщения должно быть равно SpecificPostId.");
                Assert.That(specificPostResponse.Data?.UserId, Is.EqualTo(TestData.ExpectedUserIdForSpecificPost), "UserId сообщения должно быть равно ExpectedUserIdForSpecificPost.");
                Assert.That(specificPostResponse.Data?.Title, Is.Not.Null.And.Not.Empty, "Заголовок сообщения не должен быть пустым.");
                Assert.That(specificPostResponse.Data?.Body, Is.Not.Null.And.Not.Empty, "Тело сообщения не должно быть пустым.");
            });
             _logger.LogInformation("Проверка получения пользователя с конкретным Id завершена успешно.");

            _logger.LogInformation("Запрос несуществующего пользователя с ID {Id}.", TestData.NonExistedPostId);
            var nonExistedPostResponse = await ApiUtils.GetAsync<Post>($"{ConfigData.Endpoints.Posts}/{TestData.NonExistedPostId}");
            Assert.Multiple(() =>
            {
                Assert.That(nonExistedPostResponse.Data, Is.Null, "Сообщение должно быть null.");
                Assert.That(nonExistedPostResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound), "Код состояния должен быть 404.");
            });
            _logger.LogInformation("Проверка несуществующего сообщения завершена успешно.");
            
            _logger.LogInformation("Создание нового сообщения.");
            int titleLength = TestData.TitleLength;
            int bodyLength = TestData.BodyLength;
            var newPost = new Post
            {
                UserId = TestData.PostToPostsId,
                Title = DataUtils.GenerateRandomString(titleLength),
                Body = DataUtils.GenerateRandomString(bodyLength)
            };
            var createdPostResponse =  await ApiUtils.PostAsync<Post>(ConfigData.Endpoints.Posts, newPost);
            _logger.LogInformation("Создано сообщение с ID {Id}.", createdPostResponse.Data?.Id);
            Assert.Multiple(() =>
            {
                Assert.That(createdPostResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created), "Код ответа должен быть 201.");
                Assert.That(createdPostResponse.Data, Is.Not.Null, "Ответ не должен быть null.");
                Assert.That(createdPostResponse.Data?.UserId, Is.EqualTo(newPost.UserId), "UserId в ответе должен совпадать с отправленным.");
                Assert.That(createdPostResponse.Data?.Title, Is.EqualTo(newPost.Title), "Title в ответе должен совпадать с отправленным.");
                Assert.That(createdPostResponse.Data?.Body, Is.EqualTo(newPost.Body), "Body в ответе должен совпадать с отправленным.");
                Assert.That(createdPostResponse.Data?.Id, Is.Not.Null, "Id не присутствует в ответе.");
            });
            _logger.LogInformation("Проверка созданного сообщения завершена успешно.");

            _logger.LogInformation("Запрос всех пользователей.");
            var allUsersResponse = await ApiUtils.GetAsync<List<UserData>>(ConfigData.Endpoints.Users);
            var expectedUser = TestData.UserData;
            var foundUser = allUsersResponse.Data?.FirstOrDefault(u => u.UserId == expectedUser.UserId);
            _logger.LogInformation("Получено {Count} пользователей.", allUsersResponse.Data?.Count ?? 0);
            Assert.Multiple(() =>
            {
                Assert.That(allUsersResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Код ответа должен быть 200.");
                Assert.That(foundUser, Is.Not.Null, "Пользователь с указанным UserId должен быть в списке.");
                Assert.That(foundUser, Is.EqualTo(expectedUser), "Найденный пользователь должен совпадать с ожидаемым.");
            });
             _logger.LogInformation("Проверка всех пользователей завершена успешно.");

            _logger.LogInformation("Запрос пользователя с ID {Id}.", TestData.UserData.UserId);
            var specificUserResponse = await ApiUtils.GetAsync<UserData>($"{ConfigData.Endpoints.Users}/{TestData.UserData.UserId}");
            bool userEquals = specificUserResponse.Data == expectedUser;
            _logger.LogInformation("Получен пользователь с ID: {id}.", specificUserResponse.Data?.UserId);
            Assert.Multiple(() =>
            {
                Assert.That(specificUserResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK), "Код ответа должен быть 200.");
                Assert.That(userEquals, Is.True, "Найденный пользователь должен совпадать с ожидаемым.");
            });
            _logger.LogInformation("Проверка пользователя с ID {Id} завершена успешно.", TestData.UserData.UserId);
        }
    }
}
