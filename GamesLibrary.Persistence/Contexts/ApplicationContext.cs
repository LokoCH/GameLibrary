using GamesLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesLibrary.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Creater> Creaters { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Genre[] genres = {
                new Genre{Id = Guid.NewGuid(), Name = "Action"},
                new Genre{Id = Guid.NewGuid(), Name = "Stels"},
                new Genre{Id = Guid.NewGuid(), Name = "RPG"},
                new Genre{Id = Guid.NewGuid(), Name = "Racing"},
                new Genre{Id = Guid.NewGuid(), Name = "1st person"}
            };

            modelBuilder.Entity<Genre>().HasData(genres);

            Creater[] creaters = {
                new Creater { Id = Guid.NewGuid(), Name = "Rockstar" },
                new Creater { Id = Guid.NewGuid(), Name = "Bethethda" },
                new Creater { Id = Guid.NewGuid(), Name = "Ubisoft" }
            };

            modelBuilder.Entity<Creater>().HasData(creaters);

            Game[] games = {
                new Game { Id = Guid.NewGuid(), Name = "Assassin's Creed: Black Flag", CreaterId = creaters[2].Id },
                new Game { Id = Guid.NewGuid(), Name = "Assassin's Creed: Unity", CreaterId = creaters[2].Id},
                new Game { Id = Guid.NewGuid(), Name = "GTA 5", CreaterId = creaters[0].Id },
                new Game { Id = Guid.NewGuid(), Name = "GTA: Vice City", CreaterId = creaters[0].Id },
                new Game { Id = Guid.NewGuid(), Name = "Fallout 5", CreaterId = creaters[1].Id },
                new Game { Id = Guid.NewGuid(), Name = "Skyrim", CreaterId = creaters[1].Id },

            };

            modelBuilder.Entity<Game>().HasData(games);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Genres)
                .WithMany(g => g.Games)
                .UsingEntity(gg => gg.HasData(
                    new { GamesId = games[0].Id, GenresId = genres[0].Id },
                    new { GamesId = games[0].Id, GenresId = genres[1].Id },
                    new { GamesId = games[1].Id, GenresId = genres[0].Id },
                    new { GamesId = games[1].Id, GenresId = genres[1].Id },
                    new { GamesId = games[2].Id, GenresId = genres[0].Id },
                    new { GamesId = games[2].Id, GenresId = genres[3].Id },
                    new { GamesId = games[3].Id, GenresId = genres[0].Id },
                    new { GamesId = games[3].Id, GenresId = genres[3].Id },
                    new { GamesId = games[4].Id, GenresId = genres[2].Id },
                    new { GamesId = games[4].Id, GenresId = genres[4].Id },
                    new { GamesId = games[5].Id, GenresId = genres[2].Id },
                    new { GamesId = games[5].Id, GenresId = genres[4].Id }
                    ));

        }
    }
}
