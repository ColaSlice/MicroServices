using MicroServiceProxy.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MicroServiceProxy.LoginProxy;

public class LoginProxyHandler : ILoginProxyHandler
{
    private readonly HttpClient _client;
    private const string LoginUrl = @"https://192.168.1.191:5001/api/Auth/login";
    private const string RegisterUrl = @"https://192.168.1.191:5001/api/Auth/register";
    // Do not do this in production vvv https://stackoverflow.com/questions/52939211/the-ssl-connection-could-not-be-established
    private readonly HttpClientHandler _clientHandler = new HttpClientHandler();

    public LoginProxyHandler()
    {
        // Do not do this in production vvv
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        // Do not do this in production ^^^
        _client = new HttpClient(_clientHandler);
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
}