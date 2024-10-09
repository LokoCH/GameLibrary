using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GamesLibrary.Application.Features.Genres.Commands.DeleteGenre
{
    public record DeleteGenreCommand(Guid id) : IRequest<Result>;

    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, Result>
    {
        private readonly IGenericRepository<Genre> _genreGenericRepository;
        private readonly ILogger _logger;

        public DeleteGenreCommandHandler(IGenericRepository<Genre> gameGenericRepository, ILogger<DeleteGenreCommandHandler> logger)
        {
            _genreGenericRepository = gameGenericRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Удаление жанра с ID = {request.id}");
            Genre? genre = await _genreGenericRepository.GetByIdAsync(request.id, cancellationToken);
            
            if (genre is null)
            {
                string message = $"Не удалось найти жанр с ID = {request.id}";
                _logger.LogWarning(message);
                return Result.Failure(message);
            }

            await _genreGenericRepository.DeleteAsync(genre, cancellationToken);
            _logger.LogInformation($"Удаление жанра с ID = {request.id} завершено");
            return Result.Success();
        }
    }
}
