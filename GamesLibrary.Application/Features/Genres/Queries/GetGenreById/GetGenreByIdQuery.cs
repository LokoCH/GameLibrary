using CSharpFunctionalExtensions;
using GamesLibrary.Application.Features.Games.Commands.CreateGame;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Genres.Queries.GetGenreById
{
    public record GetGenreByIdQuery(Guid Id) : IRequest<Result<GetGenreByIdDTO>>;

    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, Result<GetGenreByIdDTO>>
    {
        private readonly IGenericRepository<Genre> _genreGenericRepository;
        private readonly ILogger _logger;

        public GetGenreByIdQueryHandler(IGenericRepository<Genre> genreGenericRepository, ILogger<GetGenreByIdQueryHandler> logger)
        {
            _genreGenericRepository = genreGenericRepository;
            _logger = logger;
        }

        public async Task<Result<GetGenreByIdDTO>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос жанра с ID = {request.Id}");
            Genre? genre = await _genreGenericRepository.GetByIdAsync(request.Id, cancellationToken);

            if (genre is null)
            {
                string message = "Данные не найдены";
                _logger.LogWarning(message);
                return Result.Failure<GetGenreByIdDTO>(message);
            }

            GetGenreByIdDTO result = new GetGenreByIdDTO { Id = genre.Id, Name = genre.Name};
            _logger.LogInformation($"Ответ: {JsonSerializer.Serialize(result)}");

            return result;
        }
    }
}
