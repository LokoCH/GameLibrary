using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Games.Queries.GetGameByGenre
{
    public record GetGamesByGenreQuery(Guid genreId) : IRequest<Result<IEnumerable<GetGamesByGenreDTO>>>;

    public class GetGamesByGenreQueryHandler : IRequestHandler<GetGamesByGenreQuery, Result<IEnumerable<GetGamesByGenreDTO>>>
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger _logger;

        public GetGamesByGenreQueryHandler(IGameRepository gameRepository, ILogger<GetGamesByGenreQueryHandler> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<GetGamesByGenreDTO>>> Handle(GetGamesByGenreQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос игр по жанру с ID = {request.genreId}");

            IEnumerable<Game>? games = await _gameRepository.GetGamesByGenre(request.genreId, cancellationToken);

            if (games is null || !games.Any())
            {
                string message = "Даннеы не найдены";
                _logger.LogWarning(message);
                return Result.Failure<IEnumerable<GetGamesByGenreDTO>>(message);
            }

            IEnumerable<GetGamesByGenreDTO> result = games.Select(game => new GetGamesByGenreDTO()
            {
                Id = game.Id,
                Name = game.Name,
                Creater = game.Creater is null ? null : new GetGameByGenreCreaterDTO { Id = game.Creater.Id, Name = game.Creater.Name },
                Genres = game.Genres is null ? null : game.Genres.Select(genre => new GetGameByGenreGenreDTO { Id = genre.Id, Name = genre.Name })
            });

            _logger.LogInformation($"Ответ: {JsonSerializer.Serialize(result)}");

            return Result.Success(result);

        }
    }
}
