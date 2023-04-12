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
    private User _user;
    public LoginHandler(IConfiguration configuration, ILoginDatabaseHandler databaseHandler, ILoggerHandler loggerHandler)
    {
        _configuration = configuration;
        _databaseHandler = databaseHandler;
        _loggerHandler = loggerHandler;
        _user = new User();
    }

    public User Register(UserDto request)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        request.Password = "";
        _user.Username = request.Username;
        _user.PasswordHash = passwordHash;
        _user.Email = request.Email;
        _user.License = request.License;

        if (!_databaseHandler.UserExists(_user))
        {
            _databaseHandler.SaveUser(_user);
            return _user;
        }
        
        _user.Dispose();
        return _user;
    }

    public User Login(UserDto request)
    {
        if (_databaseHandler.ReadUser(request).Email != request.Email)
        {
            _loggerHandler.Log("Error, email is not recognised");
            _user.Dispose();
            return _user;
        }

        if (_databaseHandler.ReadUser(request).Username != request.Username)
        {
            _loggerHandler.Log("Error, username is not recognised");
            _user.Dispose();
            return _user;
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, _databaseHandler.ReadUser(request).PasswordHash))
        {
            _user.Dispose();
            return _user;
        }

        _user.Username = request.Username;
        _user.PasswordHash = _databaseHandler.ReadUser(request).PasswordHash;
        _user.Email = request.Email;
        _user.License = request.License;

        return _user;
    }
    
    public bool ValidateUser(MessageDto messageDto)
    {
        if (_databaseHandler.UsernameExists(messageDto))
        {
            return true;
        }

        return false;
    }
    
    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    
}