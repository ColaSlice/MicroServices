using System.Net;
using MicroServiceProxy.Models;
using MicroServiceProxy.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceProxy.LoginProxy;

public class LoginProxyHandler : ILoginProxyHandler
{
    private readonly HttpClient _client;
    private const string LoginUrl = @"http://login-service:5001/api/Auth/login";
    private const string RegisterUrl = @"http://login-service:5001/api/Auth/register";
    private const string ServiceStatus = @"http://login-service:5001/api/Auth/status";
    private const string ValidateUserUrl = @"http://login-service:5001/api/Auth/validateuser";
    // Do not do this in production vvv https://stackoverflow.com/questions/52939211/the-ssl-connection-could-not-be-established
    private readonly HttpClientHandler _clientHandler = new HttpClientHandler();
    private ILoggerHandler _loggerHandler;

    public LoginProxyHandler(ILoggerHandler loggerHandler)
    {
        // Do not do this in production vvv
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        // Do not do this in production ^^^
        _client = new HttpClient(_clientHandler);
        _loggerHandler = loggerHandler;
    }

    public async Task<HttpResponseMessage> Register(UserDto userDto)
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        HttpResponseMessage response;
        try
        {
            response = await _client.PostAsJsonAsync(RegisterUrl, userDto);
            userDto.Dispose();
            _client.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //_loggerHandler.Log($"Exception: {e}");
            userDto.Dispose();
            _client.Dispose();
            throw;
        }
        return response;
    }

    public async Task<HttpResponseMessage> Login(UserDto userDto)
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        var response = await _client.PostAsJsonAsync(LoginUrl, userDto);
        userDto.Dispose();
        _client.Dispose();
        return response;
    }

    public async Task<HttpResponseMessage> ValidateUser(MessageDto messageDto)
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        var response = await _client.PostAsJsonAsync(ValidateUserUrl, messageDto);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            response.StatusCode = HttpStatusCode.NotFound;
            return response;
        }
        return response;
    }

    public async Task<bool> GetStatus()
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        HttpResponseMessage response;
        try
        {
            response = await _client.GetAsync(ServiceStatus);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return false;
        }
        return true;
    }
}