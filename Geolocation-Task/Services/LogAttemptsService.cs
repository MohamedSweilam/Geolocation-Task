using Geolocation_Task.Repositories;

namespace Geolocation_Task.Services
{
    using global::Geolocation_Task.Controllers;
    using global::Geolocation_Task.Models;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    namespace Geolocation_Task.Services 
    { 

        public class LogAttemptsService : ILogAttemptsService
        {
            private static readonly ConcurrentBag<BlockedAttemptLog> _blockedAttempts = new();

            public void LogBlockedAttempt(string ipAddress, string countryCode, bool blockedStatus, string userAgent)
            {
                _blockedAttempts.Add(new BlockedAttemptLog
                {
                    IpAddress = ipAddress,
                    Timestamp = DateTime.UtcNow,
                    CountryCode = countryCode,
                    BlockedStatus = blockedStatus,
                    UserAgent = userAgent
                });
            }

            public List<BlockedAttemptLog> GetBlockedAttempts(int page, int pageSize)
            {
                return _blockedAttempts
                    .OrderByDescending(log => log.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }


        }
    }

}
