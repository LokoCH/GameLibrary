using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Games.Queries.GetAllGames
{
    public record GetAllGamesQuery(bool? isFullInfo) : IRequest<Result<IEnumerable<GetAllGamesDTO>>>;

    public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, Result<IEnumerable<GetAllGamesDTO>>>
    {
        private readonly IGenericRepository<Game> _gameGenericRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ILogger _logger;

        public GetAllGamesQueryHandler(IGenericRepository<Game> gameGenericRepository, IGameRepository gameRepository, ILogger<GetAllGamesQueryHandler> logger)
        {
            _gameGenericRepository = gameGenericRepository;
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<GetAllGamesDTO>>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Game>? games;

            if (request.isFullInfo is not null && request.isFullInfo == true)
            {
                _logger.LogInformation($"Поступил запрос на полную информацию о всех играх");
                games = await _gameRepository.GetFullInfoGamesAsync(cancellationToken);
            }
            else
            {
                _logger.LogInformation("Поступил запрос на информацию о всех играх");
                games = await _gameGenericRepository.GetAllAsync(cancellationToken);
            }

            if (games is null || !games.Any())
            {
                string message = "Данные не найдены";
                _logger.LogWarning(message);
                return Result.Failure<IEnumerable<GetAllGamesDTO>>(message);
            }

            IEnumerable<GetAllGamesDTO> result = games.Select(game => new GetAllGamesDTO()
            {
                Id = game.Id,
                Name = game.Name,
                Creater = game.Creater is null ? null : new GetAllGamesCreaterDTO { Id = game.Creater.Id, Name = game.Creater.Name },
                Genres = game.Genres is null ? null : game.Genres.Select(genre => new GetAllGamesGenreDTO { Id = genre.Id, Name = genre.Name })
            });

            _logger.LogInformation($"Ответ: {JsonSerializer.Serialize(result)}");

            return Result.Success(result);
        }
    }
}
