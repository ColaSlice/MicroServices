using MessageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Message;

public class MessageHandler : IMessageHandler
{
    // localhost:5002
    // proxy-service
    private const string SendProxyMessageUrl = @"http://proxy-service:5002/api/DatabaseProxy/savemessage";
    private string GetProxyMessageUrl = @"http://proxy-service:5002/api/DatabaseProxy/getmessages?";
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
        Console.WriteLine(messageDto.Message);
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
        Console.WriteLine(response.StatusCode);
        return response;
    }

    public async Task<HttpResponseMessage> GetMessage(string toUser)
    {
        GetProxyMessageUrl += $"toUser={toUser}";
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