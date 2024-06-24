using Newtonsoft.Json;

namespace Practice.Services.Model.Response;

public class GetAllBookDtoRes
{
    [JsonProperty("books")] 
    public List<BookingColectionDtoRes> Books { get; set; }
}