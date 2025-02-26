namespace Geolocation_Task.Repositories
{
    public interface IBlockedCountryRepository
    {
        bool AddBlockedCountry(string country);
        bool RemoveBlockedCountry(string country);
        List<string> GetAllBlockedCountries();
    }
}
