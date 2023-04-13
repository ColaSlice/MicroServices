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
        private User _user;
        private bool _usernameExists;
        private Tokens _tokens;
        private ILoginHandler _loginHandler;

        public AuthController(ILoginHandler loginHandler)
        {
            _user = new User();
            _tokens = new Tokens();
            _loginHandler = loginHandler;
        }

        [HttpPost("register")]
        public ActionResult<User> RegisterUser(UserDto request)
        {
            _user = _loginHandler.Register(request);

            if (_user.Username == "")
            {
                _user.Dispose();
                return BadRequest(_user);
            }
            
            return Ok(_user);
        }
        
        [HttpPost("login")]
        public ActionResult<Tokens> Login(UserDto request)
        {
            // TODO better login checking. You can login with any username and password, as long as the Email is the same.
            _user = _loginHandler.Login(request);
            if (_user.Email == "")
            {
                _user.Dispose();
                return NotFound("User doesn't exist");
            }

            _tokens.Token = _loginHandler.CreateToken(_user);
            
            return Ok(_tokens);
        }
        
        [HttpPost("validateuser")]
        public ActionResult<Tokens> ValidateUser(MessageDto messageDto)
        {
            Console.WriteLine("hej");
            _usernameExists = _loginHandler.ValidateUser(messageDto);
            if (!_usernameExists)
            {
                return NotFound("User doesn't exist");
            }

            return Ok(messageDto);
        }
        
        [HttpGet("status")]
        public ActionResult<bool> GetStatus()
        {
            return Ok("Running");
        }
    }
}
