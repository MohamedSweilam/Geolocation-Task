using Geolocation_Task.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Geolocation_Task.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogAttemptsService _attemptsService;

        public LogsController(ILogAttemptsService attemptsService)
        {
            _attemptsService = attemptsService;
        }
        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = _attemptsService.GetBlockedAttempts(page, pageSize);
            return Ok(logs);
        }


    }
}
        
