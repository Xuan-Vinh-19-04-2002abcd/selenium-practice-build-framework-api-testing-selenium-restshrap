using Newtonsoft.Json;

namespace Practice.Test.DataObject;

public class AccountDto
{
    [JsonProperty("username")]
    public string Username { get; set; }
    
    [JsonProperty("password")]
    public string Password { get; set; }
    
    [JsonProperty("userId")]
    public string UserId { get; set; }
}