using System.Net;
using MicroServiceProxy.DatabaseProxy;
using MicroServiceProxy.Models;
using MicroServiceProxy.LoginProxy;
using MicroServiceProxy.MessageProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace MicroServiceProxy.Controllers
{
    //ApiController needs to come before the route. Oh, and these [ApiController] is called a decorator, I think.
    [ApiController]
    [Route("api/[controller]")]
    public class LoginProxyController : ControllerBase
    {
        private static User _user = new User();
        private Tokens _tokens;
        private ILoginProxyHandler _loginProxyHandler;
        private IMessageProxyHandler _messageProxyHandler;
        private IDatabaseProxyHandler _databaseProxyHandler;
        private List<string> _services;
        
        public LoginProxyController(ILoginProxyHandler loginProxyHandler, IMessageProxyHandler messageProxyHandler, IDatabaseProxyHandler databaseProxyHandler)
        {
            _tokens = new Tokens();
            _loginProxyHandler = loginProxyHandler;
            _messageProxyHandler = messageProxyHandler;
            _databaseProxyHandler = databaseProxyHandler;
            _services = new List<string>();
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(UserDto userDto)
        {
            var response = await _loginProxyHandler.Register(userDto);
            
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                _user.Dispose();
                response.Dispose();
                return BadRequest("User already exist");
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _user.Dispose();
                response.Dispose();
                return Problem("Internal problem occured");
            }
            
            _user = (await response.Content.ReadFromJsonAsync<User>())!;
            response.Dispose();
            return Ok(_user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserDto userDto)
        {
            var response = await _loginProxyHandler.Login(userDto);
            
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _user.Dispose();
                response.Dispose();
                return BadRequest("User not found");
            }
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _user.Dispose();
                response.Dispose();
                return Problem("Internal problem occured");
            }

            _tokens.Token = await response.Content.ReadAsStringAsync();
            response.Dispose();
            return Ok(_tokens);
        }

        [HttpPost("validateuser")]
        public async Task<ActionResult> ValidateUser(UserDto userDto)
        {
            return Problem("Not implemented yet");
        }
    }
}
