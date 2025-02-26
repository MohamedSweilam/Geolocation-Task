using Geolocation_Task.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Geolocation_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IIpService _ipService;

        public IpController(IIpService ipService)
        {
            _ipService = ipService;
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
    }
}
