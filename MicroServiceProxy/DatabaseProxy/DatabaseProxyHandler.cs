using System.Net;
using MicroServiceProxy.Models;
using MicroServiceProxy.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceProxy.DatabaseProxy;

public class DatabaseProxyHandler : IDatabaseProxyHandler
{
    private readonly HttpClient _client;
    private readonly HttpClientHandler _clientHandler = new HttpClientHandler();
    private ILoggerHandler _loggerHandler;
    
    // database-service:5004
    // localhost:5004
    private const string _getLogsUrl = @"http://database-service:5004/api/Database/getlogs";
    private string _getMessageUrl = @"http://database-service:5004/api/Database/getmessages?";
    private const string _saveLogUrl = @"http://database-service:5004/api/Database/savelog";
    private const string _saveMessage = @"http://database-service:5004/api/Database/savemessage";
    private const string ServiceStatusUrl = @"http://database-service:5004/api/Database/status";

    public DatabaseProxyHandler(ILoggerHandler loggerHandler)
    {
        // Do not do this in production vvv
        _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        // Do not do this in production ^^^
        _client = new HttpClient(_clientHandler);
        _loggerHandler = loggerHandler;
    }


    public async Task<HttpResponseMessage> GetLogs()
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        HttpResponseMessage response;
        try
        {
            response = await _client.GetAsync(_getLogsUrl);
            _client.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _loggerHandler.Log($"Exception: {e}");
            _client.Dispose();
            throw;
        }

        return response;
    }

    public async Task<HttpResponseMessage> GetMessages(string toUser)
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        string temp = $"toUser={toUser}";
        _getMessageUrl += temp;
        HttpResponseMessage response;
        try
        {
            response = await _client.GetAsync(_getMessageUrl);
            _client.Dispose();
        }
        catch (Exception e)
        {
            _loggerHandler.Log($"Exception: {e}");
            _client.Dispose();
            throw;
        }
        return response;
    }

    public async Task<HttpResponseMessage> SaveLog(LogMessage logMessage)
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        HttpResponseMessage response;
        try
        {
            response = await _client.PostAsJsonAsync(_saveLogUrl, logMessage);
            _client.Dispose();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _loggerHandler.Log($"Exception: {e}");
            _client.Dispose();
            throw;
        }
        return response;
    }

    public async Task<HttpResponseMessage> SaveMessage(MessageDto messageDto)
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        HttpResponseMessage response;
        try
        {
            response = await _client.PostAsJsonAsync(_saveMessage, messageDto);
            _client.Dispose();
        }
        catch (Exception e)
        {
            _loggerHandler.Log($"Exception: {e}");
            _client.Dispose();
            throw;
        }
        return response;
    }
    
    public async Task<bool> GetStatus()
    {
        _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        HttpResponseMessage response;
        try
        {
            response = await _client.GetAsync(ServiceStatusUrl);
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
