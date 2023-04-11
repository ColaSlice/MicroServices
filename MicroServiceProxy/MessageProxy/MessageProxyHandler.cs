using MicroServiceProxy.Models;
using MicroServiceProxy.Services;

namespace MicroServiceProxy.MessageProxy;

public class MessageProxyHandler : IMessageProxyHandler
{
    private readonly HttpClient _client;
    private const string SendMessageUrl = @"http://localhost:5003/api/Message/sendmessage";
    // Do not do this in production vvv https://stackoverflow.com/questions/52939211/the-ssl-connection-could-not-be-established
    private readonly HttpClientHandler _clientHandler = new HttpClientHandler();
    private ILoggerHandler _loggerHandler;

    public MessageProxyHandler(ILoggerHandler loggerHandler)
    {
        // Do not do this in production vvv
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        // Do not do this in production ^^^
        _client = new HttpClient(_clientHandler);
        _loggerHandler = loggerHandler;
    }
    
    public async Task<HttpResponseMessage> SendMessage(MessageDto messageDto)
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        HttpResponseMessage response;
        try
        {
            response = await _client.PostAsJsonAsync(SendMessageUrl, messageDto);
            _client.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //_loggerHandler.Log($"Exception: {e}");
            _client.Dispose();
            throw;
        }
        return response;
    }
}