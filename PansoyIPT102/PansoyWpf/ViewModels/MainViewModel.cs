using Microsoft.Extensions.DependencyInjection;
using Domain.Commands;
using Domain.Models;
using Domain.Queries;
using PansoyWpf.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PansoyWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MovieStore _movieStore;

        private string _title;
        private string _director;
        private int _year;
        private string _genre;
        private Movie _selectedMovie;
        private bool _isEditMode;
        private string _searchText;
        private ObservableCollection<Movie> _filteredMovies;

        public MainViewModel(
            IServiceProvider serviceProvider,
            MovieStore movieStore)
        {
            _serviceProvider = serviceProvider;
            _movieStore = movieStore;
            _filteredMovies = new ObservableCollection<Movie>();

            AddCommand = new PansoyWpf.Commands.RelayCommand(async _ => await AddMovie(), _ => !IsEditMode);
            EditCommand = new PansoyWpf.Commands.RelayCommand(_ => EditMovie(), _ => SelectedMovie != null && !IsEditMode);
            UpdateCommand = new PansoyWpf.Commands.RelayCommand(async _ => await UpdateMovie(), _ => IsEditMode);
            DeleteCommand = new PansoyWpf.Commands.RelayCommand(async _ => await DeleteMovie(), _ => SelectedMovie != null);
            CancelCommand = new PansoyWpf.Commands.RelayCommand(_ => CancelEdit(), _ => IsEditMode);

            LoadMovies();
        }

        public ObservableCollection<Movie> Movies => _movieStore.Movies;

        public ObservableCollection<Movie> FilteredMovies
        {
            get => _filteredMovies;
            set
            {
                _filteredMovies = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterMovies();
            }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Director
        {
            get => _director;
            set { _director = value; OnPropertyChanged(); }
        }

        public int Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); }
        }

        public string Genre
        {
            get => _genre;
            set { _genre = value; OnPropertyChanged(); }
        }

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        private async void LoadMovies()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<GetAllMoviesQuery, List<Movie>>>();
                var movies = await handler.HandleAsync(new GetAllMoviesQuery());
                _movieStore.SetMovies(movies);
                FilterMovies();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading movies: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterMovies()
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

        private async Task AddMovie()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Director) || 
                Year <= 0 || string.IsNullOrWhiteSpace(Genre))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateMovieCommand>>();
                
                var command = new CreateMovieCommand
                {
                    Title = Title,
                    Director = Director,
                    Year = Year,
                    Genre = Genre
                };

                await handler.ExecuteAsync(command);
                ClearFields();
                LoadMovies();
                MessageBox.Show("Movie added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? $"\n\nInner Exception: {ex.InnerException.Message}" : "";
                MessageBox.Show($"Error adding movie: {ex.Message}{innerMessage}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditMovie()
        {
            if (SelectedMovie != null)
            {
                Title = SelectedMovie.Title;
                Director = SelectedMovie.Director;
                Year = SelectedMovie.Year;
                Genre = SelectedMovie.Genre;
                IsEditMode = true;
            }
        }

        private async Task UpdateMovie()
        {
            if (SelectedMovie == null) return;

            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Director) || 
                Year <= 0 || string.IsNullOrWhiteSpace(Genre))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<UpdateMovieCommand>>();
                
                var command = new UpdateMovieCommand
                {
                    Id = SelectedMovie.Id,
                    Title = Title,
                    Director = Director,
                    Year = Year,
                    Genre = Genre
                };

                await handler.ExecuteAsync(command);
                ClearFields();
                IsEditMode = false;
                LoadMovies();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating movie: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteMovie()
        {
            if (SelectedMovie == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedMovie.Title}'?", 
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<DeleteMovieCommand>>();
                    
                    var command = new DeleteMovieCommand { Id = SelectedMovie.Id };
                    await handler.ExecuteAsync(command);
                    ClearFields();
                    LoadMovies();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting movie: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelEdit()
        {
            ClearFields();
            IsEditMode = false;
        }

        private void ClearFields()
        {
            Title = string.Empty;
            Director = string.Empty;
            Year = 0;
            Genre = string.Empty;
            SelectedMovie = null;
        }
    }
}
