using Geolocation_Task.Repositories;
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

        public IpController(IIpService ipService, IBlockedCountryRepository blockedCountry , ITemoprallyBlocked temoprallyBlocked)
        {
            _ipService = ipService;
            _blockedCountry = blockedCountry;
            _temoprallyBlocked = temoprallyBlocked;
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

            if (_blockedCountry.CheckCountry(result.country_code))
                return Conflict($"Blocked");
            if(_temoprallyBlocked.IsCountryTemporarilyBlocked(result.country_code))
                return Conflict($" Temporalily Blocked");


            return Ok(result);

        }

    }
}
