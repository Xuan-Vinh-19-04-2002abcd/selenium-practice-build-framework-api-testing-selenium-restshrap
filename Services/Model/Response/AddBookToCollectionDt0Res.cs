using Newtonsoft.Json;

namespace Practice.Services.Model.Response;

public class AddBookToCollectionDt0Res
{
    [JsonProperty("books")] 
    public List<BookId> Books { get; set; }
}

public class BookId
{
    [JsonProperty("isbn")]
    public string Isbn { get; set; }
}