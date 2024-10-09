using Foxminded.Library.DAL;
using Foxminded.Library.Domain.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Filters;

namespace Foxminded.Library.Application.ServiceProviderInititalizations;

public static class ServiceProviderInit
{
    public static ServiceProvider GetServiceProvider(this IServiceCollection services)
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Prod";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json")
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return services
               .AddSingleton<IConfiguration>(configuration)
               .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
               .Configure<BookSearchCriteria>(options => configuration.GetSection("Filter").Bind(options))
               .AddDbContext<LibraryDbContext>(options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer(configuration.GetConnectionString("DB")))
               .GetServicesTransient()
               .BuildServiceProvider();
    }
}
