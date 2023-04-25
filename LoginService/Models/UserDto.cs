namespace LoginService.Models;

public record UserDto : IDisposable
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string License { get; set; }
    
    public void Dispose()
    {
        Username = string.Empty;
        Password = string.Empty;
        Email = string.Empty;
        License = string.Empty;
    }
}