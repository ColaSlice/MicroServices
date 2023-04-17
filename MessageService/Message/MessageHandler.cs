using MessageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Message;

public class MessageHandler : IMessageHandler
{
    private const string SendProxyMessageUrl = @"http://localhost:5002/api/Proxy/savemessage";
    private string GetProxyMessageUrl = @"http://localhost:5002/api/DatabaseProxyConstroller/getmessages?";
    // Do not do this in production vvv https://stackoverflow.com/questions/52939211/the-ssl-connection-could-not-be-established
    private readonly HttpClientHandler _clientHandler = new HttpClientHandler();
    private readonly HttpClient _client;
    public MessageHandler()
    {
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        // Do not do this in production ^^^
        _client = new HttpClient(_clientHandler);
    }
    public async Task<HttpResponseMessage> SendMessage(MessageDto messageDto)
    {
        HttpResponseMessage response;
        try
        {
            response = await _client.PostAsJsonAsync(SendProxyMessageUrl, messageDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return response;
    }

    public async Task<HttpResponseMessage> GetMessage(string toUser, string user)
    {
        GetProxyMessageUrl += $"toUser={toUser}&user={user}";
        HttpResponseMessage response;
        try
        {
            response = await _client.GetAsync(GetProxyMessageUrl);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return response;
    }
}