namespace Domain.Commands
{
    public class CreateMovieCommand
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
    }
}
