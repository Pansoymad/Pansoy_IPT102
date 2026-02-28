using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Commands;
using Domain.Models;
using Domain.Queries;
using Framework.Commands;
using Framework.Queries;
using PansoyWpf.Stores;
using PansoyWpf.ViewModels;

namespace PansoyWpf.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MovieStore>();
                services.AddSingleton<NavigationStore>();
                
                services.AddTransient<ICommandHandler<CreateMovieCommand>, CreateMovieCommandHandler>();
                services.AddTransient<ICommandHandler<UpdateMovieCommand>, UpdateMovieCommandHandler>();
                services.AddTransient<ICommandHandler<DeleteMovieCommand>, DeleteMovieCommandHandler>();
                services.AddTransient<IQueryHandler<GetAllMoviesQuery, List<Movie>>, GetAllMoviesQueryHandler>();
                
                services.AddTransient<MainViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));
            });

            return hostBuilder;
        }
    }
}
