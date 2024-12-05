namespace RestApiClient.Models
{
    public class ConfigModel
    {
        public required string BaseUrl { get; set; }
        public required EndpointDetails Endpoints { get; set; }

        public class EndpointDetails
        {
            public required string Posts { get; set; }
            public required string Users { get; set; }
        }
    }
}
