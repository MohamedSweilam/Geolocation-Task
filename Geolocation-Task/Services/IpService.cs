using Geolocation_Task.Repositories;
using System.Net;
using System.Text.Json;


namespace Geolocation_Task.Services
{
    public class IpService : IIpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IpService(HttpClient httpClient, IConfiguration config , IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse> GetCountryCode(string? ip=null)
        {
            if (string.IsNullOrEmpty(ip))
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();


            ip = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                  ?? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            

            if (IPAddress.TryParse(ip, out _))
            {
                var apiKey = _config["IpStack:IpStackKey"];
                var apiUrl = _config["IpStack:IpStackUrl"];
                var response = await _httpClient.GetAsync($"{apiUrl}{ip}?access_key={apiKey}");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);
                if(data.Ip == null)
                {
                    data.Ip = ip;
                   

                }
                return data!;
            }
            else
            {
                return null;
         
            }
        }

        

        public class ApiResponse
        {
            public string? Ip { get; set; } 
            public string country_name { get; set; } = default!;
            public string country_code { get; set; } = default!;
            public string continent_name { get; set; } = default!;
           

        }
    }
}

