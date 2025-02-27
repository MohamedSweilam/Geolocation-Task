using Geolocation_Task.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Geolocation_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IBlockedCountryRepository _countryService;
        private readonly ITemoprallyBlocked _temoprallyBlock;
        private readonly IIpService _ipService;

        public CountriesController(IBlockedCountryRepository countryService, ITemoprallyBlocked temoprallyBlock, IIpService ipService)
        {
            _countryService = countryService;
            _temoprallyBlock = temoprallyBlock;
            _ipService = ipService;
        }
        [HttpGet("blocked")]
        public IActionResult GetAll([FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            var data = _countryService.GetAllBlockedCountries(page, pageSize);

            return data == null ? NotFound() : Ok(data);
        }


        [HttpPost("block")]
        public IActionResult AddBlockedCountry([FromBody] string countryCode)
        {
            var result = _countryService.AddBlockedCountry(countryCode);
            return result == true ? Ok($"{countryCode} has been blocked") : Conflict($"{countryCode} is already blocked");
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult GetBlockedCountry(string countryCode)
        {
            var data = _countryService.RemoveBlockedCountry(countryCode);

            return data == true ? Ok() : NotFound();
        }


        [HttpPost("temporal-block")]

        public async Task<IActionResult> TemporarilyBlockCountry([FromBody] Result res)
        {
           
            
            if (res.CountryCode==null)

            {
                return BadRequest("Invalid country code format.");
            }           

            if (res.DurationMinutes < 1 || res.DurationMinutes > 1440)
            {
                return BadRequest("Duration must be between 1 and 1440 minutes (24 hours).");
            }

            // Check if the country is already blocked (expired blocks will be removed automatically)
            if (_temoprallyBlock.IsCountryTemporarilyBlocked(res.CountryCode))
            {
                return Conflict("Country is already temporarily blocked.");
            }

            // Block the country
            _temoprallyBlock.TemporarilyBlockCountry(res.CountryCode, res.DurationMinutes);

            return Ok($"{res.CountryCode} has been blocked for {res.DurationMinutes} minutes.");
        }

        public class Result 
        {
            public int DurationMinutes { get; set; }
            public string CountryCode { get; set; }
        }


    }
}
