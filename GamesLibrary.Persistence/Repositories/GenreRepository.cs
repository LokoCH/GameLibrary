using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using GamesLibrary.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GamesLibrary.Persistence.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationContext _context;

        public GenreRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetGenresByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellation)
        {
            return await _context.Genres.AsNoTracking().Where(genre => ids.Contains(genre.Id)).ToListAsync(cancellation);
        }
    }
}
