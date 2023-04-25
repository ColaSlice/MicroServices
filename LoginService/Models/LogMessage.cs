namespace LoginService.Models;

public record LogMessage
{
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}