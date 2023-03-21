using LoginService.Database;
using LoginService.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace LoginService.Login;
public class LoginHandler : ILoginHandler
{
    private ILoginDatabaseHandler _databaseHandler;
    private readonly IConfiguration _configuration;
    private User _user;
    public LoginHandler(IConfiguration configuration, ILoginDatabaseHandler databaseHandler)
    {
        _configuration = configuration;
        _user = new User();
        _databaseHandler = databaseHandler;
    }

    public User Register(UserDto request)
    {
        // Vi får kodeordet som plaintext, men da det er over https, så er det
        // allerede krypteret når det bliver sendt.
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        // vvvvv Vi vil ikke have kodeordet til at ligge og flyde rundt.
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
            _user.Dispose();
            return _user;
        }

        if (_databaseHandler.ReadUser(request).Username != request.Username)
        {
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