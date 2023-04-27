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
        public async Task<ActionResult<MessageDto>> SendMessage(MessageDto messageDto)
        {
            var response = await _messageHandler.SendMessage(messageDto);
            return Ok(response);
        }
        
        [HttpPost("getmessage")]
        public async Task<ActionResult<MessageDto>> GetMessage(string toUser)
        {
            var response = await _messageHandler.GetMessage(toUser);
            return Ok(response);
        }

        [HttpGet("status")]
        public ActionResult<bool> GetStatus()
        {
            return Ok("Running");
        }
    }
}