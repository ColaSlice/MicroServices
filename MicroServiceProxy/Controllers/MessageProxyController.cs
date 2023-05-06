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
public class MessageProxyController : ControllerBase
{
    private static User _user = new User();
    private ILoginProxyHandler _loginProxyHandler;
    private IMessageProxyHandler _messageProxyHandler;
    private IDatabaseProxyHandler _databaseProxyHandler;
    private List<string> _services;
        
    public MessageProxyController(ILoginProxyHandler loginProxyHandler, IMessageProxyHandler messageProxyHandler, IDatabaseProxyHandler databaseProxyHandler)
    {
        _loginProxyHandler = loginProxyHandler;
        _messageProxyHandler = messageProxyHandler;
        _databaseProxyHandler = databaseProxyHandler;
        _services = new List<string>();
    }
    
    [HttpPost("sendmessage")]
    public async Task<ActionResult<string>> SendMessage(MessageDto messageDto)
    {
        var userResponse = await _loginProxyHandler.ValidateUser(messageDto);
        if (userResponse.StatusCode != HttpStatusCode.OK)
        {
            userResponse.Dispose();
            return NotFound();
        }
        var response = await _messageProxyHandler.SendMessage(messageDto);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            response.Dispose();
            return Problem("Internal problem occured");
        }
            
        return Ok(await response.Content.ReadAsStringAsync());
    }
}