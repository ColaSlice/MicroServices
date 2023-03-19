namespace LoginService.Models;

public class Tokens
{
    public string Token { get; set; } = string.Empty;

    public void Dispose()
    {
        Token = string.Empty;
    }
}