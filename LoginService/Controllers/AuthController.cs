using Microsoft.AspNetCore.Mvc;
using LoginService.Models;
using LoginService.Login;
using LoginService.Services;

namespace LoginService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private User? _user;
        private readonly Tokens _tokens;
        private readonly ILoginHandler _loginHandler;

        public AuthController(ILoginHandler loginHandler)
        {
            _user = new User();
            _tokens = new Tokens();
            _loginHandler = loginHandler;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(UserDto request)
        {
            _user = await _loginHandler.Register(request);

            if (_user == null)
            {
                return Conflict("Either The User Already Exists Or An Error Occurred");
            }
            
            return Ok("User Registered");
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<Tokens>> Login(UserDto request)
        {
            _user = await _loginHandler.Login(request);
            
            if (_user == null)
            {
                return NotFound("User not found");
            }

            _tokens.Token = _loginHandler.CreateToken(_user);
            
            return Ok(_tokens);
        }
        
        [HttpPost("validateuser")]
        public async Task<ActionResult> ValidateUser(MessageDto messageDto)
        {
            if (await _loginHandler.ValidateUser(messageDto)) return Ok("Validated");
            
            return NotFound("User doesn't exist");
        }
        
        [HttpGet("status")]
        public ActionResult<bool> GetStatus()
        {
            return Ok("Running");
        }
    }
}
