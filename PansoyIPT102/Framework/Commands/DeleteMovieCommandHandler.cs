using Domain.Commands;

namespace Framework.Commands
{
    public class DeleteMovieCommandHandler : ICommandHandler<DeleteMovieCommand>
    {
        private readonly MoviesDbContext _context;

        public DeleteMovieCommandHandler(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(DeleteMovieCommand command)
        {
            var movie = await _context.Movies.FindAsync(command.Id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}
