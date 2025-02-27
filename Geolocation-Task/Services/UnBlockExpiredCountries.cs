
using Geolocation_Task.Repositories;

namespace Geolocation_Task.Services
{
    public class UnBlockExpiredCountries : BackgroundService
    {
        private readonly ITemoprallyBlocked _temoprallyBlocked;

        public UnBlockExpiredCountries(ITemoprallyBlocked temoprallyBlocked)
        {
            _temoprallyBlocked = temoprallyBlocked;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Checking for expired country blocks...");
                _temoprallyBlocked.RemoveExpiredBlocks();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
