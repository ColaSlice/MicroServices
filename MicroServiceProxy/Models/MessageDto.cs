namespace MicroServiceProxy.Models
{
    public class MessageDto : IDisposable
    {
        public string ToUser { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

        public void Dispose()
        {
            Message = string.Empty;
        }
    }
}
