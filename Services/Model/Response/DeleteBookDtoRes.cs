using Newtonsoft.Json;

namespace Practice.Services.Model.Response;

public class DeleteBookDtoRes
{
    [JsonProperty("userId")]
    public string UserId { get; set; }
    [JsonProperty("isbn")]
    public string Isbn { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
}