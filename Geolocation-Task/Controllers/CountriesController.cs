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

        public CountriesController(IBlockedCountryRepository countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("blocked")]
        public IActionResult GetAll()
        {
            var data =_countryService.GetAllBlockedCountries();

            return data ==null ? NotFound() : Ok(data);
        }

        [HttpPost("block")]
        public IActionResult AddBlockedCountry([FromBody]string countryCode)
        {
            var result = _countryService.AddBlockedCountry(countryCode);
            return result == true ? Ok($"{countryCode} has been blocked") : Conflict($"{countryCode} is already blocked") ;
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult GetBlockedCountry(string countryCode)
        {
            var data = _countryService.RemoveBlockedCountry(countryCode);
            
            return data == true ? Ok(): NotFound();
        }
    }
}
