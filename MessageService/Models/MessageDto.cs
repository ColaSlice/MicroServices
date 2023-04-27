namespace MessageService.Models
{
    public class MessageDto
    {
        public string ToUser { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}