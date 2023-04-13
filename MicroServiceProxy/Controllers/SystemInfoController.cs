using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private List<string> _services;
        private List<string> _endpoints;
        
        public SystemInfoController(ILoginProxyHandler loginProxyHandler, IMessageProxyHandler messageProxyHandler)
        {
            _loginProxyHandler = loginProxyHandler;
            _messageProxyHandler = messageProxyHandler;
            _services = new List<string>();
            _endpoints = new List<string>();
        }

        [HttpGet("getendpoints")]
        public async Task<ActionResult<List<string>>> GetEndpoints()
        {
            _endpoints.Add("POST: api/Proxy/register");
            _endpoints.Add("POST: api/Proxy/login");
            _endpoints.Add("POST: api/Proxy/sendmessage");
            _endpoints.Add("GET: api/SystemInfo/getendpoints");
            _endpoints.Add("GET: api/SystemInfo/getstatus");
            _endpoints.Add("GET: api/Database/getlogs");
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
            return Ok(_services);
        }
    }
}
