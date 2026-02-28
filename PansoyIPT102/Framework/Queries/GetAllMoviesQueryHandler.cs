using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Queries;

namespace Framework.Queries
{
    public class GetAllMoviesQueryHandler : IQueryHandler<GetAllMoviesQuery, List<Movie>>
    {
        private readonly MoviesDbContext _context;

        public GetAllMoviesQueryHandler(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> HandleAsync(GetAllMoviesQuery query)
        {
            var movieDtos = await _context.Movies.ToListAsync();
            return movieDtos.Select(dto => new Movie
            {
                Id = dto.Id,
                Title = dto.Title,
                Director = dto.Director,
                Year = dto.Year,
                Genre = dto.Genre
            }).ToList();
        }
    }
}
