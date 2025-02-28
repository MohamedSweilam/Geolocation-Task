using Geolocation_Task.Controllers;

namespace Geolocation_Task.Repositories
{
    public interface ILogAttemptsService
    {
        
        
            void LogBlockedAttempt(string ipAddress, string countryCode, bool blockedStatus, string userAgent);
            List<BlockedAttemptLog> GetBlockedAttempts(int page, int pageSize);
        

    }
}