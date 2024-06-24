using Newtonsoft.Json;

namespace Practice.Services.Model.Request;

public class DeleteBookDtoReq
{  
    [JsonProperty("isbn")]
    public string Isbn { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
}