namespace MicroServiceProxy.Models
{
    public class MessageDto : IDisposable
    {
        public string User { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }

        public void Dispose()
        {
            Message = string.Empty;
        }
    }
}