using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Genres.Queries.GetAllGenres
{
    public record GetAllGenresQuery() : IRequest<Result<IEnumerable<GetAllGenresDTO>>>;

    public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, Result<IEnumerable<GetAllGenresDTO>>>
    {
        private readonly IGenericRepository<Genre> _genreGenericRepository;
        private readonly ILogger _logger;

        public GetAllGenresQueryHandler(IGenericRepository<Genre> gameGenericRepository, ILogger<GetAllGenresQueryHandler> logger)
        {
            _genreGenericRepository = gameGenericRepository;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<GetAllGenresDTO>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Поступил запрос на информацию о всех жанрах");
            IEnumerable<Genre>? genres = await _genreGenericRepository.GetAllAsync(cancellationToken);

            if (genres is null || !genres.Any())
            {
                string message = "Данные не найдены";
                _logger.LogWarning(message);
                return Result.Failure<IEnumerable<GetAllGenresDTO>>(message);
            }

            IEnumerable<GetAllGenresDTO> result = genres.Select(game => new GetAllGenresDTO()
            {
                Id = game.Id,
                Name = game.Name,
            });

            _logger.LogInformation($"Ответ: {JsonSerializer.Serialize(result)}");

            return Result.Success(result);
        }
    }
}
