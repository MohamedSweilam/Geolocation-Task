namespace Geolocation_Task.Repositories
{
    public interface ITemoprallyBlocked
    {
        bool TemporarilyBlockCountry(string countryCode, int durationMinutes);
        bool IsCountryTemporarilyBlocked(string countryCode);

        void RemoveExpiredBlocks();
    }
}