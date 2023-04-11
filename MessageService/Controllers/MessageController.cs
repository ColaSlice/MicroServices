using Microsoft.AspNetCore.Mvc;
using MessageService.Models;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        public MessageController()
        {
            
        }

        [HttpPost("sendmessage")]
        public ActionResult<string> SendMessage(MessageDto messageDto)
        {
            Console.Error.WriteLine("asd");
            return Ok("abc");
        }
    }
}