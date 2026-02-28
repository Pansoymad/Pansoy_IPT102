using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Framework
{
    public class MoviesDbContextFactory : IDesignTimeDbContextFactory<MoviesDbContext>
    {
        public MoviesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MoviesDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Pansoy;Trusted_Connection=True;");
            
            return new MoviesDbContext(optionsBuilder.Options);
        }
    }
}
