using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseService.Database;
using DatabaseService.Enums;
using DatabaseService.Models;
using MessageService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private IDatabaseHandler _databaseHandler;
        public DatabaseController(IDatabaseHandler databaseHandler)
        {
            _databaseHandler = databaseHandler;
        }

        // GET: api/Database/getlogs
        [HttpGet("getlogs")]
        public List<LogMessage> GetLogs()
        {
            return _databaseHandler.ReadLogs(null);
        }
        
        // GET: api/Database/getmessages
        [HttpGet("getmessages")]
        public List<MessageDto> GetMessages(string toUser, string user)
        {
            return _databaseHandler.ReadMessage(null, toUser, user);
        }

        // POST: api/Database/savelog
        [HttpPost("savelog")]
        public async Task<ActionResult> SaveLog(LogMessage logMessage)
        {
            _databaseHandler.Save(Types.Log, null, logMessage);
            return Ok();
        }
        
        // POST: api/Database/savemessage
        [HttpPost("savemessage")]
        public async Task<ActionResult> SaveMessage(MessageDto messageDto)
        {
            _databaseHandler.Save(Types.Message, messageDto, null);
            return Ok();
        }
        
        // DELETE: api/Database/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
        [HttpGet("status")]
        public ActionResult<bool> GetStatus()
        {
            return Ok("Running");
        }
    }
}
