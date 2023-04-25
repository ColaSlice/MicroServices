namespace LoginService.Models;

public record Tokens
{
    public string Token { get; set; } = string.Empty;

    public void Dispose()
    {
        Token = string.Empty;
    }
}