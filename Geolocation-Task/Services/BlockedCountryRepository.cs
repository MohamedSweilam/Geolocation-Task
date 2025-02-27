using Geolocation_Task.Repositories;
using System.Collections.Concurrent;
using System.Linq;

namespace Geolocation_Task.Services
{
    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, bool> _blockedCountries = new();
        public bool AddBlockedCountry(string country)
            => _blockedCountries.TryAdd(country.ToUpper(), true);

        public List<string> GetAllBlockedCountries(int? page = null, int? pageSize = null)
        {
            if(page!=null || pageSize != null) 
            {
                return _blockedCountries
                .OrderByDescending(k => k.Key)  // Sort by country code
                .Skip((page.Value - 1) * pageSize.Value) // Skip previous pages
                .Take(pageSize.Value)  // Limit results per page
                .Select(k => k.Key)    // Extract only country codes
                .ToList();
            }
            else
            {
                return _blockedCountries.Keys.ToList();
            }

            
        }


        public bool RemoveBlockedCountry(string country)
        => _blockedCountries.TryRemove(country.ToUpper(),out _);
        public bool CheckCountry(string countryCode)
        {
            return _blockedCountries.ContainsKey(countryCode);

        }
        
    }

    
}
