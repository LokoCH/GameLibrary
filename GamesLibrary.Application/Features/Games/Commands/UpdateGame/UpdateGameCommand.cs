using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Application.Features.Games.Commands.UpdateGame
{
    public record UpdateGameCommand : IRequest<Result<UpdateGameDTO>>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public Guid CreaterId { get; set; }
        [Required]
        public IEnumerable<Guid> GenreIds { get; set; } = null!;
    }

    public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, Result<UpdateGameDTO>>
    {
        private readonly IGenericRepository<Game> _gameGenericRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ILogger _logger;

        public UpdateGameCommandHandler(IGenericRepository<Game> gameGenericRepository, IGenreRepository genreRepository, ILogger<UpdateGameCommandHandler> logger, IGameRepository gameRepository)
        {
            _gameGenericRepository = gameGenericRepository;
            _genreRepository = genreRepository;
            _logger = logger;
            _gameRepository = gameRepository;
        }

        public async Task<Result<UpdateGameDTO>> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на обновление игры с ID = {request.Id}");
            Game? oldGame = await _gameGenericRepository.GetByIdAsync(request.Id, cancellationToken);

            if (oldGame is null)
            {
                string message = $"Не удалось найти игру с ID = {request.Id}";
                _logger.LogWarning(message);
                return Result.Failure<UpdateGameDTO>(message);
            }

            IEnumerable<Genre> genres = await _genreRepository.GetGenresByIdsAsync(request.GenreIds, cancellationToken);

            if (genres is null || !genres.Any())
            {
                string errorMessage = "Жанры не найдены в базе";
                _logger.LogWarning(errorMessage);
                return Result.Failure<UpdateGameDTO>(errorMessage);
            }

            oldGame.CreaterId = request.CreaterId;
            oldGame.Genres = genres;
            oldGame.Name = request.Name;

            var validateResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(oldGame, new ValidationContext(oldGame), validateResults))
            {
                string error = validateResults.First().ErrorMessage!;
                _logger.LogWarning($"Не пройдена валидация: {error}");
                return Result.Failure<UpdateGameDTO>(error);
            }

            Game? updatedGame = await _gameRepository.UpdateManyToMany(oldGame, cancellationToken);

            if (updatedGame is null)
            {
                string errorMessage = "Не удалось обновить";
                _logger.LogWarning(errorMessage);
                return Result.Failure<UpdateGameDTO>(errorMessage);
            }

            UpdateGameDTO updatedGameDTO = new UpdateGameDTO
            {
                Id = updatedGame.Id,
                Name = updatedGame.Name,
                CreaterId = updatedGame.CreaterId,
                Genres = updatedGame.Genres.Select(genre => new UpdateGameGenresDTO { Id = genre.Id, Name = genre.Name })
            };
            _logger.LogInformation($"Обновление завершено {updatedGameDTO}");
            return updatedGameDTO;
        }
    }
}
