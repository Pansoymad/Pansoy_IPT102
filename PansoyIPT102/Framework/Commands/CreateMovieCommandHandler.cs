using Domain.Commands;
using Framework.DTOs;

namespace Framework.Commands
{
    public class CreateMovieCommandHandler : ICommandHandler<CreateMovieCommand>
    {
        private readonly MoviesDbContext _context;

        public CreateMovieCommandHandler(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(CreateMovieCommand command)
        {
            var movieDto = new MovieDto
            {
                Title = command.Title,
                Director = command.Director,
                Year = command.Year,
                Genre = command.Genre
            };

            _context.Movies.Add(movieDto);
            await _context.SaveChangesAsync();
        }
    }
}
