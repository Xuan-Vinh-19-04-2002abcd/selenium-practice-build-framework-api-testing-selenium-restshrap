using RestSharp;
using RestSharp.Authenticators;

namespace Practice.Core;

public class APIClient
{
    private readonly RestClient _client;
    public RestRequest Request;
    private RestClientOptions _restClientOptions;

    public APIClient(RestClient client)
    {
        _client = client;
        Request = new RestRequest();
    }

    public APIClient(string url)
    {
        var options = new RestClientOptions(url);
        _client = new RestClient(options, configureSerialization: s => s.UseDefaultSerializers());
        Request = new RestRequest();
    }
    public APIClient(RestClientOptions options)
    {
        _client = new RestClient(options, configureSerialization: s => s.UseDefaultSerializers());
        Request = new RestRequest();
    }

    public APIClient SetBasicAuthentication(string username, string password)
    {
        _restClientOptions.Authenticator = new HttpBasicAuthenticator(username, password);
        return new APIClient(_restClientOptions);
    }

    public APIClient AddDefaultHeaders(Dictionary<string, string> headers)
    {
        _client.AddDefaultHeaders(headers);
        return this;
    }

    public APIClient CreateRequest(string resource = "")
    {
        Request = new RestRequest(resource);
        return this;
    }

    public APIClient AddHeader(string key, string value)
    {
        Request.AddHeader(key, value);
        return this;
    }

    public APIClient ClearAuthenticatot()
    {
        _restClientOptions.Authenticator = null;
        return new APIClient(_restClientOptions);
    }

    public APIClient AddAuthorizationHeader(string value)
    {
        return AddHeader("Authorization", value);
    }
    public APIClient AddContentTypeHeader(string value)
    {
        return AddHeader("Content-Type", value);
    }

    public APIClient AddParamater(string name, string value)
    {
        Request.AddParameter(name, value);
        return this;
    }

    public APIClient AddBody(object obj, string contentType = null)
    {
        Request.AddBody(obj, contentType);
        return this;
    }
    public APIClient AddJsonBody(object obj)
    {
        Request.AddJsonBody(obj);
        return this;
    }

    public async Task<RestResponse> ExecuteyGetAsync()
    {
        return await _client.ExecuteGetAsync(Request);
    }
    
    public async Task<RestResponse<T>> ExecuteyGetAsync<T>()
    {
        return await _client.ExecuteGetAsync<T>(Request);
    }
    
    public async Task<RestResponse> ExecutePostAsync()
    {
        return await _client.ExecutePostAsync(Request);
    }
    
    public async Task<RestResponse<T>> ExecutePostAsync<T>()
    {
        return await _client.ExecutePostAsync<T>(Request);
    }
    
    public async Task<RestResponse> ExecutePutAsync()
    {
        return await _client.ExecutePutAsync(Request);
    }
    
    public async Task<RestResponse<T>> ExecutePutAsync<T>()
    {
        return await _client.ExecutePutAsync<T>(Request);
    }
    
    public async Task<RestResponse<T>> ExecuteDeleteAsync<T>()
    {
        Request.Method = Method.Delete;
        return await _client.ExecuteAsync<T>(Request);
    }
    public RestResponse<T> ExecuteDelete<T>()
    {
        Request.Method = Method.Delete;
        return  _client.Execute<T>(Request);
    }
    
    
    
}