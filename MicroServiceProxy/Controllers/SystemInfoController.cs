using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroServiceProxy.DatabaseProxy;
using MicroServiceProxy.LoginProxy;
using MicroServiceProxy.MessageProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceProxy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemInfoController : ControllerBase
    {
        private ILoginProxyHandler _loginProxyHandler;
        private IMessageProxyHandler _messageProxyHandler;
        private IDatabaseProxyHandler _databaseProxyHandler;
        private List<string> _services;
        private List<string> _endpoints;
        
        public SystemInfoController(ILoginProxyHandler loginProxyHandler, IMessageProxyHandler messageProxyHandler, IDatabaseProxyHandler databaseProxyHandler)
        {
            _loginProxyHandler = loginProxyHandler;
            _messageProxyHandler = messageProxyHandler;
            _databaseProxyHandler = databaseProxyHandler;
            _services = new List<string>();
            _endpoints = new List<string>();
        }

        [HttpGet("getendpoints")]
        public async Task<ActionResult<List<string>>> GetEndpoints()
        {
            _endpoints.Add("POST: api/LoginProxy/register");
            _endpoints.Add("POST: api/LoginProxy/login");
            _endpoints.Add("POST: api/LoginProxy/validateUser");
            _endpoints.Add("================================");
            _endpoints.Add("GET:  api/DatabaseProxy/getmessages");
            _endpoints.Add("POST: api/DatabaseProxy/savemessage");
            _endpoints.Add("GET:  api/DatabaseProxy/getlogs");
            _endpoints.Add("POST: api/DatabaseProxy/savelog");
            _endpoints.Add("================================");
            _endpoints.Add("POST: api/MessageProxy/sendmessage");
            _endpoints.Add("================================");
            _endpoints.Add("GET:  api/SystemInfo/getendpoints");
            _endpoints.Add("GET:  api/SystemInfo/getstatus");
            
            
            return Ok(_endpoints);
        }

        [HttpGet("getstatus")]
        public async Task<ActionResult<List<string>>> GetStatus()
        {
            if (! await _messageProxyHandler.GetStatus())
            {
                _services.Add("MessageService: Not Running");
            }
            else
            {
                _services.Add("MessageService: Running");
            }
            
            if (! await _loginProxyHandler.GetStatus())
            {
                _services.Add("LoginService: Not Running");
            }
            else
            {
                _services.Add("LoginService: Running");
            }

            if (!await _databaseProxyHandler.GetStatus())
            {
                _services.Add("DatabaseService: Not Running");
            }
            else
            {
                _services.Add("DatabaseService: Running");
            }
            return Ok(_services);
        }
    }
}
