using Microsoft.EntityFrameworkCore;
using Domain.Commands;

namespace Framework.Commands
{
    public class UpdateMovieCommandHandler : ICommandHandler<UpdateMovieCommand>
    {
        private readonly MoviesDbContext _context;

        public UpdateMovieCommandHandler(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(UpdateMovieCommand command)
        {
            var movie = await _context.Movies.FindAsync(command.Id);
            if (movie != null)
            {
                movie.Title = command.Title;
                movie.Director = command.Director;
                movie.Year = command.Year;
                movie.Genre = command.Genre;
                await _context.SaveChangesAsync();
            }
        }
    }
}
