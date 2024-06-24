using System.Net;
using FluentAssertions;
using Newtonsoft.Json;
using Practice.Core;
using Practice.Core.Extensions;
using Practice.Core.ShareData;
using Practice.Services.Model.Response;
using Practice.Test.Constants;
using Practice.Test.DataObject;
using RestSharp;

namespace Practice.Services.Service;

public class UserService
{
    private readonly APIClient _client;

    public UserService(APIClient apiClient)
    {
        _client = apiClient;
    }

    public async Task<RestResponse<GetDeatailUserDtoRes>> GetUserSuccess(string userId, string token)
    {
        var response = await _client.CreateRequest(string.Format(EndPointConstant.GetUseEndpoint, userId))
            .AddHeader("accept", "application/json")
            .AddAuthorizationHeader(token)
            .ExecuteyGetAsync<GetDeatailUserDtoRes>();
        return response;
    }
    public async Task<RestResponse<UnAuthorizationDtoRes>> GetUserWithUnAuthorization(string userId)
    {
        var response = await _client.CreateRequest(string.Format(EndPointConstant.GetUseEndpoint, userId))
            .AddHeader("accept", "application/json")
            .ExecuteyGetAsync<UnAuthorizationDtoRes>();
        return response;
    }
    
    public async Task<RestResponse> TryHardGenerateTokenAysnc(string username, string password)
    {
        var response = await _client.CreateRequest("Account/v1/GenerateToken")
            .AddHeader("accept", "application/json")
            .AddJsonBody(new
            {
                userName = username,
                password = password
            }).ExecutePostAsync();
        return response;
    }
    public async Task StoreTokenAsync(string accountKey, AccountDto account)
    {
        if (DataStorage.GetData(accountKey) is null)
        {
            var response = await TryHardGenerateTokenAysnc(account.Username, account.Password);
            response.VerifyStatusCodeOk();
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            DataStorage.SetData(accountKey,"Bearer" +" "+ result["token"]);
        }
    }

    public string GetToken(string accountKey)   
    {
        if (DataStorage.GetData(accountKey) is null)
        {
            throw new Exception("Token is not store with account");
        }
        return (string)DataStorage.GetData(accountKey);
    }
}