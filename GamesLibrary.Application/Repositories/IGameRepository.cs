using GamesLibrary.Domain.Entities;

namespace GamesLibrary.Application.Repositories
{
    public interface IGameRepository
    {
        Task<Game?> GetFullInfoByGameIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Game>?> GetFullInfoGamesAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Game>?> GetGamesByGenre(Guid genreId, CancellationToken cancellationToken);
    }
}
