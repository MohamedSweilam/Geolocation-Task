using Geolocation_Task.Models;
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
        public   IActionResult GetAllBlockedCountries([FromQuery] string? search = null, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
        {
            // Validate pagination parameters
            if ((page.HasValue && !pageSize.HasValue) || (!page.HasValue && pageSize.HasValue))
            {
                return BadRequest("Both page and pageSize must be provided together.");
            }

            var blockedCountries =  _countryService.GetAllBlockedCountries(search, page, pageSize);

            if (!blockedCountries.Any())
            {
                return NotFound("No blocked countries found matching the criteria.");
            }

            return Ok(blockedCountries);
        }


        [HttpPost("block")]
        public IActionResult AddBlockedCountry([FromBody] BlockedCountryRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.CountryCode) || string.IsNullOrWhiteSpace(req.CountryName))
            {
                return BadRequest("Country code and country name cannot be empty.");
            }

            var result = _countryService.AddBlockedCountry(req.CountryCode.ToUpper(), req.CountryName);
            return result ? Ok($"{req.CountryName} ({req.CountryCode}) has been blocked") : Conflict($"{req.CountryName} is already blocked.");
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
                return  BadRequest("Invalid country code format.");
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
