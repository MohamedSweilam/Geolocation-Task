
using Geolocation_Task.Repositories;
using Geolocation_Task.Services;
using Geolocation_Task.Services.Geolocation_Task.Services;

namespace Geolocation_Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IBlockedCountryRepository, BlockedCountryRepository>();
            builder.Services.AddScoped<IIpService, IpService>();
            builder.Services.AddControllers();
            builder.Services.AddHostedService<UnBlockExpiredCountries>();
            builder.Services.AddSingleton<ITemoprallyBlocked, TemporallyBlocked>();
            builder.Services.AddSingleton<ILogAttemptsService, LogAttemptsService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
