using Geolocation_Task.Repositories;
using System.Collections.Concurrent;

namespace Geolocation_Task.Services
{
    public class TemporallyBlocked : ITemoprallyBlocked
    {

        private readonly ConcurrentDictionary<string, DateTime> _temporallyBlockedCountries = new();

        public bool TemporarilyBlockCountry(string countryCode, int durationMinutes)
        {
            if (_temporallyBlockedCountries.ContainsKey(countryCode))
                return false; 

            _temporallyBlockedCountries[countryCode] = DateTime.UtcNow.AddMinutes(durationMinutes);
            return true;
        }

        public bool IsCountryTemporarilyBlocked(string countryCode)
        {
            if (_temporallyBlockedCountries.TryGetValue(countryCode, out DateTime expiryTime))
            {
                if (expiryTime > DateTime.UtcNow)
                    return true;

                _temporallyBlockedCountries.TryRemove(countryCode, out _); 
            }
            return false;
        }

        public void RemoveExpiredBlocks()
        {
            foreach (var country in _temporallyBlockedCountries)
            {
                if (country.Value <= DateTime.UtcNow)
                    _temporallyBlockedCountries.TryRemove(country.Key, out _);
            }
        }
    }
}
