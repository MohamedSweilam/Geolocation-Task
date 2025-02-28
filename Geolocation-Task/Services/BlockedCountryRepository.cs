using Geolocation_Task.Repositories;
using System.Collections.Concurrent;
using System.Linq;

namespace Geolocation_Task.Services
{
    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, string> _blockedCountries = new();

        public bool AddBlockedCountry(string countryCode, string countryName)
            => _blockedCountries.TryAdd(countryCode.ToUpper(), countryName);

        public List<KeyValuePair<string, string>> GetAllBlockedCountries(string? search = null, int? page = null, int? pageSize = null)
        {
            var query = _blockedCountries.AsEnumerable();

           
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();
                query = query.Where(c => c.Key.ToLower().Contains(searchLower) || c.Value.ToLower().Contains(searchLower));
            }

           
            query = query.OrderBy(k => k.Key);

            
            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }

        public bool RemoveBlockedCountry(string countryCode)
            => _blockedCountries.TryRemove(countryCode.ToUpper(), out _);

        public bool CheckCountry(string countryCode)
            => _blockedCountries.ContainsKey(countryCode);
    }


}

    

