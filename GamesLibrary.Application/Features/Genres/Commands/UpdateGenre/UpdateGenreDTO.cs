namespace GamesLibrary.Application.Features.Genres.Commands.UpdateGenre
{
    public class UpdateGenreDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

}
