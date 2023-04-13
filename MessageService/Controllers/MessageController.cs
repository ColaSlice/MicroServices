using MessageService.Message;
using Microsoft.AspNetCore.Mvc;
using MessageService.Models;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private IMessageHandler _messageHandler;
        public MessageController(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        [HttpPost("sendmessage")]
        public ActionResult<string> SendMessage(MessageDto messageDto)
        {
            return Ok(_messageHandler.SendMessage(messageDto));
        }
        
        [HttpPost("getmessage")]
        public ActionResult<string> GetMessage(MessageDto messageDto)
        {
            return Ok(_messageHandler.GetMessage());
        }

        [HttpGet("status")]
        public ActionResult<bool> GetStatus()
        {
            return Ok("Running");
        }
    }
}