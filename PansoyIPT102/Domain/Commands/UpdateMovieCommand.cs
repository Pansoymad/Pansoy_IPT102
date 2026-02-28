namespace Domain.Commands
{
    public class UpdateMovieCommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
    }
}
