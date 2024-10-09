using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Application.Features.Games.Commands.UpdateGame
{
    public record UpdateGameCommand : IRequest<Result<UpdateGameVM>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CreaterId { get; set; }
        public IEnumerable<Guid> GenreIds { get; set; } = null!;
    }

    public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, Result<UpdateGameVM>>
    {
        private readonly IGenericRepository<Game> _gameGenericRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger _logger;

        public UpdateGameCommandHandler(IGenericRepository<Game> gameGenericRepository, IGenreRepository genreRepository, ILogger<UpdateGameCommandHandler> logger)
        {
            _gameGenericRepository = gameGenericRepository;
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<Result<UpdateGameVM>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на обновление игры с ID = {request.Id}");
            Game? oldGame = await _gameGenericRepository.GetByIdAsync(request.Id, cancellationToken);

            if (oldGame is null)
            {
                string message = $"Не удалось найти игру с ID = {request.Id}";
                _logger.LogWarning(message);
                return Result.Failure<UpdateGameVM>(message);
            }

            IEnumerable<Genre> genres = await _genreRepository.GetGenresByIdsAsync(request.GenreIds, cancellationToken);
            Game newGame = new Game { Id = request.Id, Name = request.Name, CreaterId = request.CreaterId, Genres = genres };

            var validateResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(newGame, new ValidationContext(newGame), validateResults))
            {
                string error = validateResults.First().ErrorMessage!;
                _logger.LogWarning($"Не пройдена валидация: {error}");
                return Result.Failure<UpdateGameVM>(error);
            }

            await _gameGenericRepository.UpdateAsync(newGame, cancellationToken);

            UpdateGameVM updateGameVm = new UpdateGameVM
            {
                OldGame = new UpdateGameDTO
                {
                    Id = oldGame.Id,
                    Name = oldGame.Name,
                    CreaterId = oldGame.CreaterId,
                    Genres = oldGame.Genres.Select(genre => new UpdateGameGenresDTO { Id = genre.Id, Name = genre.Name })
                },
                NewGame = new UpdateGameDTO
                {
                    Id = newGame.Id,
                    Name = newGame.Name,
                    CreaterId = newGame.CreaterId,
                    Genres = newGame.Genres.Select(genre => new UpdateGameGenresDTO { Id = genre.Id, Name = genre.Name })
                }
            };
            _logger.LogInformation($"Обновление завершено {updateGameVm}");
            return updateGameVm;
        }
    }
}
