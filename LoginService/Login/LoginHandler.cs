using LoginService.Database;
using LoginService.Models;
using LoginService.Services;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LoginService.Login;
public class LoginHandler : ILoginHandler
{
    private readonly ILoginDatabaseHandler _databaseHandler;
    private readonly IConfiguration _configuration;
    private readonly ILoggerHandler _loggerHandler;
    private User _user;
    public LoginHandler(IConfiguration configuration, ILoginDatabaseHandler databaseHandler, ILoggerHandler loggerHandler)
    {
        _configuration = configuration;
        _databaseHandler = databaseHandler;
        _loggerHandler = loggerHandler;
        _user = new User();
    }

    public async Task<User?> Register(UserDto request)
    {
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        request.Password = "";
        _user.Username = request.Username;
        _user.PasswordHash = passwordHash;
        _user.Email = request.Email;
        _user.License = request.License;
        
        if (await _databaseHandler.UserExists(_user.Email))
        {
            return null;
        }
        
        if (await _databaseHandler.SaveUser(_user)) return _user;
        
        _databaseHandler.Dispose();
        _user.Dispose();
        return null!;
    }

    public async Task<User> Login(UserDto request)
    {
        if (!await _databaseHandler.UserExists(request.Email))
        {
            request.Dispose();
            return null!;
        }
        
        _user = (await _databaseHandler.ReadUser(request))!;
        
        if (BCrypt.Net.BCrypt.Verify(request.Password, _user!.PasswordHash)) return _user;
        
        request.Dispose();
        return null!;
    }
    
    public async Task<bool> ValidateUser(MessageDto messageDto)
    {
        return await _databaseHandler.UserExists(messageDto.Email);
    }
    
    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.UserData, user.License)
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