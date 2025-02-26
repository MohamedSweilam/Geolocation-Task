using Geolocation_Task.Repositories;
using System.Collections.Concurrent;

namespace Geolocation_Task.Services
{
    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, bool> _blockedCountries = new();
        public bool AddBlockedCountry(string country)
            => _blockedCountries.TryAdd(country.ToUpper(), true);

        public List<string> GetAllBlockedCountries()
        =>_blockedCountries.Keys.ToList();

        public bool RemoveBlockedCountry(string country)
        => _blockedCountries.TryRemove(country.ToUpper(),out _);
    }
}
