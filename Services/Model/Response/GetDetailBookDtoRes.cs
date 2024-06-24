using Newtonsoft.Json;

namespace Practice.Services.Model.Response;

public class GetDetailBookDtoRes
{
    [JsonProperty("isbn")] 
    public string Isbn { get; set; }
    [JsonProperty("title")] 
    public string Title { get; set; }
    [JsonProperty("subTitle")] 
    public string SubTitle { get; set; }
    [JsonProperty("author")] 
    public string Author { get; set; }
    [JsonProperty("publish_date")] 
    public string PublishDate { get; set; }
    [JsonProperty("publisher")] 
    public string Publisher { get; set; }
    [JsonProperty("pages")] 
    public int Pages { get; set; }
    [JsonProperty("description")] 
    public string Description { get; set; }
    [JsonProperty("website")] 
    public string Website { get; set; }
    public void Display()
    {
        Console.WriteLine("Book Details:");
        Console.WriteLine($"ISBN: {Isbn}");
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Subtitle: {SubTitle}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Publish Date: {PublishDate}");
        Console.WriteLine($"Publisher: {Publisher}");
        Console.WriteLine($"Pages: {Pages}");
        Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Website: {Website}");
        Console.WriteLine();
    }
}