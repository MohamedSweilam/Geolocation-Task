using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Geolocation_Task.Repositories
{
    public interface IIpService
    {

        Task<object> GetCountryCode(string ip);
    }
}
