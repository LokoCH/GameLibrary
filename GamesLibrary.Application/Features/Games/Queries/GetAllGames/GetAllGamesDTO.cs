namespace GamesLibrary.Application.Features.Games.Queries.GetAllGames
{
    public class GetAllGamesDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public GetAllGamesCreaterDTO? Creater { get; set; }
        public IEnumerable<GetAllGamesGenreDTO>? Genres { get; set; } = null;
    }

    public class GetAllGamesGenreDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetAllGamesCreaterDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}