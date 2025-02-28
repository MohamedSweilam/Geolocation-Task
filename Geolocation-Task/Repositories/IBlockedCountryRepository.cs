namespace Geolocation_Task.Repositories
{
    public interface IBlockedCountryRepository
    {
        bool AddBlockedCountry(string countryCode,string countryName);
        bool RemoveBlockedCountry(string countryCode);
        List<KeyValuePair<string, string>> GetAllBlockedCountries(string? search,int? page = null, int? pageSize = null);
        bool CheckCountry(string countryCode);
    }
}
