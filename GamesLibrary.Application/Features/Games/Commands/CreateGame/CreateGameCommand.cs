using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Games.Commands.CreateGame
{
    public record CreateGameCommand() : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public Guid CreaterId { get; set; }
        public IEnumerable<Guid> GenreIds { get; set; } = [];

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Result<Guid>>
    {
        private readonly IGenericRepository<Game> _gameGenericRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger _logger;

        public CreateGameCommandHandler(IGenericRepository<Game> genericGameRepository, IGenreRepository genreRepository, ILogger<CreateGameCommandHandler> logger)
        {
            _gameGenericRepository = genericGameRepository;
            _genreRepository = genreRepository;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на добавление игры: {request}");

            IEnumerable<Genre> genres = await _genreRepository.GetGenresByIdsAsync(request.GenreIds, cancellationToken);

            Game game = new Game
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CreaterId = request.CreaterId,
                Genres = genres
            };

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(game, new ValidationContext(game), validationResults, true))
            {
                string error = validationResults.First().ErrorMessage!;
                _logger.LogWarning($"Ошибка валидации: {error}");
                return Result.Failure<Guid>(error);
            }

            Game newGame = await _gameGenericRepository.CreateAsync(game, cancellationToken);
            _logger.LogInformation($"Добавлена игра: {newGame}");

            return newGame.Id;
        }
    }
}
