namespace MicroServiceProxy.Models
{
    public class MessageDto : IDisposable
    {
        public string Message { get; set; } = string.Empty;

        public void Dispose()
        {
            Message = string.Empty;
        }
    }
}