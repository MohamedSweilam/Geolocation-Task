namespace Geolocation_Task.Repositories
{
    public interface IBlockedCountryRepository
    {
        bool AddBlockedCountry(string country);
        bool RemoveBlockedCountry(string country);
        List<string> GetAllBlockedCountries(int? page = null, int? pageSize = null);
        bool CheckCountry(string countryCode);
    }
}
