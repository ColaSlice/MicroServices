using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MessageService.Models;
using MessageService.MessageHandlers;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private Message _mm = new Message();

        // GET: api/Message
        [HttpGet]
        public async Task<ActionResult<Message[]>> Get()
        {
            //MessageHandler messageHandler = new();
            Message[] messages = new Message[1];
            _mm.id = 0;
            _mm.userMessage = "asd";
            messages[0] = _mm;
            return messages;
        }

        // GET: api/Message/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Message
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Message/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Message/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
