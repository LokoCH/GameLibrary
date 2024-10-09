namespace GamesLibrary.Application.Features.Games.Queries.GetGameById
{
    public class GetGameByIdDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public GetGameByIdCreaterDTO? Creater { get; set; }
        public IEnumerable<GetGameByIdGenreDTO>? Genres { get; set; } = null!;
    }

    public class GetGameByIdGenreDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetGameByIdCreaterDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}