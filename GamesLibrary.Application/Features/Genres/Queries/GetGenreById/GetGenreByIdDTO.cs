namespace GamesLibrary.Application.Features.Genres.Queries.GetGenreById
{
    public class GetGenreByIdDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}