using Newtonsoft.Json;

namespace Practice.Services.Model.Response;

public class BadRequestDtoRes
{
    [JsonProperty("code")] 
    public int Code { get; set; }
    [JsonProperty("message")] 
    public string Message { get; set; }
}