using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Framework;

namespace PansoyWpf.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<MoviesDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                }, ServiceLifetime.Transient);
            });

            return hostBuilder;
        }
    }
}
