using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Application.Features.Genres.Commands.UpdateGenre
{
    public record UpdateGenreCommand : IRequest<Result<UpdateGenreDTO>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, Result<UpdateGenreDTO>>
    {
        private readonly IGenericRepository<Genre> _genreGenericRepository;
        private readonly ILogger _logger;

        public UpdateGenreCommandHandler(IGenericRepository<Genre> genreGenericRepository, ILogger<UpdateGenreCommandHandler> logger)
        {
            _genreGenericRepository = genreGenericRepository;
            _logger = logger;
        }

        public async Task<Result<UpdateGenreDTO>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на обновление игры с ID = {request.Id}");
            Genre? oldGenre = await _genreGenericRepository.GetByIdAsync(request.Id, cancellationToken);

            if (oldGenre is null)
            {
                string message = $"Не удалось найти жанр с ID = {request.Id}";
                _logger.LogWarning(message);
                return Result.Failure<UpdateGenreDTO>(message);
            }

            oldGenre.Id = request.Id;
            oldGenre.Name = request.Name;

            var validateResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(oldGenre, new ValidationContext(oldGenre), validateResults))
            {
                string error = validateResults.First().ErrorMessage!;
                _logger.LogWarning($"Не пройдена валижация: {error}");
                return Result.Failure<UpdateGenreDTO>(error);
            }

            await _genreGenericRepository.UpdateAsync(oldGenre, cancellationToken);

            UpdateGenreDTO updateGameVm = new UpdateGenreDTO
            {
                Id = oldGenre.Id,
                Name = oldGenre.Name,
            };
            _logger.LogInformation($"Обновление завершено {updateGameVm}");
            return updateGameVm;
        }
    }
}
