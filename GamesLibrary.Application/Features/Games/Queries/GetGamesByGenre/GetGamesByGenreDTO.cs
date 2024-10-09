namespace GamesLibrary.Application.Features.Games.Queries.GetGameByGenre
{
    public class GetGamesByGenreDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public GetGameByGenreCreaterDTO? Creater { get; set; }
        public IEnumerable<GetGameByGenreGenreDTO>? Genres { get; set; } = null!;
    }

    public class GetGameByGenreGenreDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetGameByGenreCreaterDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}