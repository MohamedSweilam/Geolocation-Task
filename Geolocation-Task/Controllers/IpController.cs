using Geolocation_Task.Repositories;
using Geolocation_Task.Services.Geolocation_Task.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Geolocation_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IIpService _ipService;
        private readonly IBlockedCountryRepository _blockedCountry;
        private readonly ITemoprallyBlocked _temoprallyBlocked;
        private readonly ILogAttemptsService _logAttemptsService; 

        public IpController(
            IIpService ipService,
            IBlockedCountryRepository blockedCountry,
            ITemoprallyBlocked temoprallyBlocked,
            ILogAttemptsService logAttemptsService) 
        {
            _ipService = ipService;
            _blockedCountry = blockedCountry;
            _temoprallyBlocked = temoprallyBlocked;
            _logAttemptsService = logAttemptsService; 
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> GetCountry([FromQuery] string ip)
        {
            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ip))
                return BadRequest("Unable to determine the IP address.");

            var result = await _ipService.GetCountryCode(ip);
            return Ok(result);
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ip))
                return BadRequest("Unable to determine the IP address.");

            var result = await _ipService.GetCountryCode(ip);

            bool isBlocked = _blockedCountry.CheckCountry(result.country_code);
            bool isTemporarilyBlocked = _temoprallyBlocked.IsCountryTemporarilyBlocked(result.country_code);

            // Log the attempt before returning response
            _logAttemptsService.LogBlockedAttempt(
                ip,
                result.country_code,
                isBlocked || isTemporarilyBlocked,
                HttpContext.Request.Headers["User-Agent"].ToString()
            );

            if (isBlocked)
                return Conflict("Blocked");

            if (isTemporarilyBlocked)
                return Conflict("Temporarily Blocked");

            return Ok(result);
        }
    }

}

