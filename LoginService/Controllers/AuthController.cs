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
        private bool _usernameExists;
        private static Tokens _tokens;
        private ILoginHandler _loginHandler;

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
                return BadRequest("User Already Exists");
            }
            
            return Ok();
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<Tokens>> Login(UserDto request)
        {
            // TODO better login checking. You can login with any username and password, as long as the Email is the same.
            _user = await _loginHandler.Login(request);
            if (_user.Email == "")
            {
                return NotFound("User not found");
            }

            _tokens.Token = _loginHandler.CreateToken(_user);
            
            return Ok(_tokens);
        }
        
        [HttpPost("validateuser")]
        public async Task<ActionResult> ValidateUser(MessageDto messageDto)
        {
            _usernameExists = await _loginHandler.ValidateUser(messageDto);
            if (!_usernameExists)
            {
                return NotFound("User doesn't exist");
            }

            return Ok("Validated");
        }
        
        [HttpGet("status")]
        public ActionResult<bool> GetStatus()
        {
            return Ok("Running");
        }
    }
}
