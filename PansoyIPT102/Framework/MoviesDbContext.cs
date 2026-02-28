using Microsoft.EntityFrameworkCore;
using Framework.DTOs;

namespace Framework
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {
        }

        public DbSet<MovieDto> Movies { get; set; }
    }
}
