using GamesLibrary.Domain.Abstractions;
using GamesLibrary.Domain.Entities;

namespace GamesLibrary.Application.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>?> GetAllAsync(CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}
