using System.Net;
using MicroServiceProxy.DatabaseProxy;
using MicroServiceProxy.LoginProxy;
using MicroServiceProxy.MessageProxy;
using MicroServiceProxy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceProxy.Controllers;

//ApiController needs to come before the route. Oh, and these [ApiController] is called a decorator, I think.
[ApiController]
[Route("api/[controller]")]
public class DatabaseProxyConstroller : ControllerBase
{
    private static User _user = new User();
    private Tokens _tokens;
    private ILoginProxyHandler _loginProxyHandler;
    private IMessageProxyHandler _messageProxyHandler;
    private IDatabaseProxyHandler _databaseProxyHandler;
    private List<string> _services;
        
    public DatabaseProxyConstroller(ILoginProxyHandler loginProxyHandler, IMessageProxyHandler messageProxyHandler, IDatabaseProxyHandler databaseProxyHandler)
    {
        _tokens = new Tokens();
        _loginProxyHandler = loginProxyHandler;
        _messageProxyHandler = messageProxyHandler;
        _databaseProxyHandler = databaseProxyHandler;
        _services = new List<string>();
    }
    
    [HttpGet("getmessages")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(string toUser, string user)
    {
        var response = await _databaseProxyHandler.GetMessages(toUser, user);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            response.Dispose();
            return NotFound();
        }
        return Ok(await response.Content.ReadFromJsonAsync<List<MessageDto>>());
    }
    
    [HttpPost("savemessage")]
    public async Task<ActionResult> SaveMessage(MessageDto messageDto)
    {
        var response = await _databaseProxyHandler.SaveMessage(messageDto);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            response.Dispose();
            return NotFound();
        }
        return Ok();
    }
    
    [HttpGet("getlogs")]
    public async Task<ActionResult<List<LogMessage>>> GetLogs()
    {
        var response = await _databaseProxyHandler.GetLogs();
        if (response.StatusCode != HttpStatusCode.OK)
        {
            response.Dispose();
            return NotFound();
        }
        return Ok(await response.Content.ReadFromJsonAsync<List<LogMessage>>());
    }

    [HttpPost("savelog")]
    public async Task<ActionResult> SaveLog(LogMessage logMessage)
    {
        var response = await _databaseProxyHandler.SaveLog(logMessage);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            response.Dispose();
            return NotFound();
        }
        return Ok();
    }
}