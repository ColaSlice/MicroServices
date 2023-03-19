namespace LoginService.Models;

public class User : IDisposable
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? License { get; set; } = string.Empty;

    public void Dispose()
    {
        Username = string.Empty;
        PasswordHash = string.Empty;
        Email = string.Empty;
        License = string.Empty;
    }
}