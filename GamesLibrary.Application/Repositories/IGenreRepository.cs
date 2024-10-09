using GamesLibrary.Domain.Entities;

namespace GamesLibrary.Application.Repositories
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    }
}
