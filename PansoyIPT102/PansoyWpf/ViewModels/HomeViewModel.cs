using Microsoft.Extensions.DependencyInjection;
using Domain.Models;
using Domain.Queries;
using PansoyWpf.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PansoyWpf.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MovieStore _movieStore;
        private readonly NavigationStore _navigationStore;
        private string _searchText;
        private ObservableCollection<Movie> _filteredMovies;

        public HomeViewModel(
            IServiceProvider serviceProvider,
            MovieStore movieStore,
            NavigationStore navigationStore)
        {
            _serviceProvider = serviceProvider;
            _movieStore = movieStore;
            _navigationStore = navigationStore;
            _filteredMovies = new ObservableCollection<Movie>();

            SearchCommand = new PansoyWpf.Commands.RelayCommand(_ => PerformSearch());
            ViewMovieCommand = new PansoyWpf.Commands.RelayCommand(param => NavigateToManager());

            LoadMovies();
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                PerformSearch();
            }
        }

        public ObservableCollection<Movie> FilteredMovies
        {
            get => _filteredMovies;
            set
            {
                _filteredMovies = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand ViewMovieCommand { get; }

        private async void LoadMovies()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<GetAllMoviesQuery, List<Movie>>>();
                var movies = await handler.HandleAsync(new GetAllMoviesQuery());
                _movieStore.SetMovies(movies);
                PerformSearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading movies: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PerformSearch()
        {
            FilteredMovies.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var movie in _movieStore.Movies)
                {
                    FilteredMovies.Add(movie);
                }
            }
            else
            {
                var searchLower = SearchText.ToLower();
                var filtered = _movieStore.Movies.Where(m =>
                    m.Title.ToLower().Contains(searchLower) ||
                    m.Director.ToLower().Contains(searchLower) ||
                    m.Genre.ToLower().Contains(searchLower));

                foreach (var movie in filtered)
                {
                    FilteredMovies.Add(movie);
                }
            }
        }

        private void NavigateToManager()
        {
            _navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        }
    }
}
