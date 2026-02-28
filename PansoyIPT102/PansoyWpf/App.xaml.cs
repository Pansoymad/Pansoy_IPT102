using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PansoyWpf.HostBuilders;
using System.IO;
using System.Windows;

namespace PansoyWpf
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .AddDbContext()
                .AddServices()
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            // Apply migrations automatically on startup
            try
            {
                using (var scope = _host.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<Framework.MoviesDbContext>();
                    dbContext.Database.Migrate();
                    MessageBox.Show("Database initialized successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database migration error: {ex.Message}\n\nInner: {ex.InnerException?.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
