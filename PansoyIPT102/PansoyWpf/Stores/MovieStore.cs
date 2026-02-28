using Domain.Models;
using System.Collections.ObjectModel;

namespace PansoyWpf.Stores
{
    public class MovieStore
    {
        private ObservableCollection<Movie> _movies = new ObservableCollection<Movie>();

        public ObservableCollection<Movie> Movies => _movies;

        public event Action MoviesChanged;

        public void SetMovies(List<Movie> movies)
        {
            _movies.Clear();
            foreach (var movie in movies)
            {
                _movies.Add(movie);
            }
            MoviesChanged?.Invoke();
        }

        public void AddMovie(Movie movie)
        {
            _movies.Add(movie);
            MoviesChanged?.Invoke();
        }

        public void UpdateMovie(Movie movie)
        {
            var existingMovie = _movies.FirstOrDefault(m => m.Id == movie.Id);
            if (existingMovie != null)
            {
                existingMovie.Title = movie.Title;
                existingMovie.Director = movie.Director;
                existingMovie.Year = movie.Year;
                existingMovie.Genre = movie.Genre;
            }
            MoviesChanged?.Invoke();
        }

        public void RemoveMovie(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                _movies.Remove(movie);
            }
            MoviesChanged?.Invoke();
        }
    }
}
