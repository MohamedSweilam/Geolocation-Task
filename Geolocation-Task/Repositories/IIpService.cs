using Microsoft.AspNetCore.Mvc.ApiExplorer;
using static Geolocation_Task.Services.IpService;

namespace Geolocation_Task.Repositories
{
    public interface IIpService
    {

        Task<ApiResponse> GetCountryCode(string? ip=null);
    }
}
