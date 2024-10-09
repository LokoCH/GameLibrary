using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GamesLibrary.Application.Features.Games.Commands.DeleteGame
{
    public record DeleteGameCommand(Guid id) : IRequest<Result>;

    public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, Result>
    {
        private readonly IGenericRepository<Game> _gameGenericRepository;
        private readonly ILogger _logger;

        public DeleteGameCommandHandler(IGenericRepository<Game> gameGenericRepository, ILogger<DeleteGameCommandHandler> logger)
        {
            _gameGenericRepository = gameGenericRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Удаление игры с ID = {request.id}");
            Game? game = await _gameGenericRepository.GetByIdAsync(request.id, cancellationToken);

            if (game is null)
            {
                string message = $"Не удалось найти игру с ID = {request.id}";
                _logger.LogWarning(message);
                return Result.Failure(message);
            }

            await _gameGenericRepository.DeleteAsync(game, cancellationToken);
            _logger.LogInformation($"Удаление игры с ID = {request.id} завершено");
            return Result.Success();
        }
    }
}
