using Newtonsoft.Json;

namespace Practice.Services.Model.Response;

public class GetDeatailUserDtoRes
{
    [JsonProperty("userId")] 
    public string UserId { get; set; }
    [JsonProperty("username")] 
    public string Username { get; set; }
    [JsonProperty("books")] 
    public List<BookingColectionDtoRes> Books { get; set; }
}