using System.Text.Json.Serialization;

namespace RestApiClient.Models
{
    public record Geo
    {
        public string? Lat { get; set; }
        public string? Lng { get; set; }
    }

    public record Address
    {
        public string? Street { get; set; }
        public string? Suite { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }
        public Geo? Geo { get; set; }
    }

    public record Company
    {
        public string? Name { get; set; }
        public string? CatchPhrase { get; set; }
        public string? Bs { get; set; }
    }

    public record UserData
    {
        [JsonPropertyName("id")]
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public Address? Address { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public Company? Company { get; set; }
    }
}
