using System.Diagnostics;
using LoginService.Database;
using LoginService.Models;
using LoginService.Services;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace LoginService.Login;
public class LoginHandler : ILoginHandler
{
    private ILoginDatabaseHandler _databaseHandler;
    private readonly IConfiguration _configuration;
    private ILoggerHandler _loggerHandler;
    private static User _user;
    //private Stopwatch _stopwatch;
    public LoginHandler(IConfiguration configuration, ILoginDatabaseHandler databaseHandler, ILoggerHandler loggerHandler)
    {
        //_stopwatch = new Stopwatch();
        _configuration = configuration;
        _databaseHandler = databaseHandler;
        _loggerHandler = loggerHandler;
        _user = new User();
    }

    public async Task<User> Register(UserDto request)
    {
        Task<bool> userExists = _databaseHandler.UserExists(request.Email, null);
        _user.Username = request.Username;
        request.Password = "";
        _user.Email = request.Email;
        _user.License = request.License;

        if (!userExists.GetAwaiter().GetResult())
        {
            _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await _databaseHandler.SaveUser(_user);
            return _user;
        }
        
        _user.Dispose();
        _databaseHandler.Dispose();
        return _user;
    }

    public User Login(UserDto request)
    {
        Task<bool> isVeryfied = Verify(request.Password, _databaseHandler.ReadUser(request).PasswordHash);
        User checkUser = _databaseHandler.ReadUser(request);
        if (checkUser.Email != request.Email)
        {
            //_loggerHandler.Log("Error, email is not recognised");
            _user.Dispose();
            _databaseHandler.Dispose();
            return _user;
        }

        if (checkUser.Username != request.Username)
        {
            //_loggerHandler.Log("Error, username is not recognised");
            _user.Dispose();
            _databaseHandler.Dispose();
            return _user;
        }

        if (!isVeryfied.GetAwaiter().GetResult())
        {
            _user.Dispose();
            _databaseHandler.Dispose();
            return _user;
        }

        _user.Username = request.Username;
        _user.PasswordHash = _databaseHandler.ReadUser(request).PasswordHash;
        _user.Email = request.Email;
        _user.License = request.License;
        _databaseHandler.Dispose();
        
        return _user;
    }

    private async Task<bool> Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
    
    public async Task<bool> ValidateUser(MessageDto messageDto)
    {
        return await _databaseHandler.UserExists(null, messageDto.User);
    }
    
    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Username)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    
}