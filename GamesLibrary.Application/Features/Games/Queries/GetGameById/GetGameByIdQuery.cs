using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Games.Queries.GetGameById
{
    public record GetGameByIdQuery(Guid Id, bool? isFullInfo) : IRequest<Result<GetGameByIdDTO>>;

    public class GetGameByIdQueryHandler : IRequestHandler<GetGameByIdQuery, Result<GetGameByIdDTO>>
    {
        private readonly IGenericRepository<Game> _gameGenericRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ILogger _logger;

        public GetGameByIdQueryHandler(IGenericRepository<Game> repository, IGameRepository gameRepository, ILogger<GetGameByIdQueryHandler> logger)
        {
            _gameGenericRepository = repository;
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<Result<GetGameByIdDTO>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            Game? game;
            if (request.isFullInfo is not null && request.isFullInfo is true)
            {
                _logger.LogInformation($"Запрос полной информации игры с ID = {request.Id}");
                game = await _gameRepository.GetFullInfoByGameIdAsync(request.Id, cancellationToken);
            }
            else
            {
                _logger.LogInformation($"Запрос игры с ID = {request.Id}");
                game = await _gameGenericRepository.GetByIdAsync(request.Id, cancellationToken);
            }

            if (game is null)
            {
                string message = "Данные не найдены";
                _logger.LogWarning(message);
                return Result.Failure<GetGameByIdDTO>(message);
            }

            IEnumerable<GetGameByIdGenreDTO>? genres = game.Genres is null ? null : game.Genres.Select(genre => new GetGameByIdGenreDTO { Id = genre.Id, Name = genre.Name });
            GetGameByIdCreaterDTO? creater = game.Creater is null ? null : new GetGameByIdCreaterDTO { Id = game.Creater.Id, Name = game.Creater.Name };
            var result = new GetGameByIdDTO { Id = game.Id, Name = game.Name, Creater = creater, Genres = genres };
            _logger.LogInformation($"Ответ: {JsonSerializer.Serialize(result)}");

            return result;
        }
    }
}
