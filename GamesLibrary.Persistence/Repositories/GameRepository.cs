using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using GamesLibrary.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GamesLibrary.Persistence.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationContext _context;

        public GameRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>?> GetGamesByGenre(Guid genreId, CancellationToken cancellation)
        {
            IEnumerable<Game> games = await _context.Games.AsNoTracking()
                .Where(game => game.Genres.Select(genre => genre.Id).Contains(genreId))
                .Include(game => game.Creater)
                .Include(game => game.Genres)
                .ToListAsync(cancellation);
            return games;
        }

        public async Task<Game?> GetFullInfoByGameIdAsync(Guid id, CancellationToken cancellation)
        {
            Game? game = await _context.Games
                .AsNoTracking()
                .Include(game => game.Genres)
                .Include(game => game.Creater)
                .FirstOrDefaultAsync(game => game.Id == id, cancellation);
            return game;
        }

        public async Task<IEnumerable<Game>?> GetFullInfoGamesAsync(CancellationToken cancellation)
        {
            IEnumerable<Game>? games = await _context.Games
                .AsNoTracking()
                .Include(game => game.Genres)
                .Include(game => game.Creater)
                .ToListAsync(cancellation);

            return games;
        }
    }
}
