using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Abstractions;
using GamesLibrary.Domain.Entities;
using GamesLibrary.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GamesLibrary.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext _context;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellation)
        {
            await _context.AddAsync(entity, cancellation);
            await _context.SaveChangesAsync(cancellation);
            return entity;
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellation)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task<IEnumerable<T>?> GetAllAsync(CancellationToken cancellation)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellation);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellation)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellation);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellation)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
