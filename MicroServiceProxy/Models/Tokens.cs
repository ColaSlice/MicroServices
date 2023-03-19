namespace MicroServiceProxy.Models;

public class Tokens : IDisposable
{
    public string Token { get; set; } = string.Empty;

    public void Dispose()
    {
        Token = string.Empty;
    }
}