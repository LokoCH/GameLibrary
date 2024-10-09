using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Application.Features.Genres.Commands.UpdateGenre
{
    public record UpdateGenreCommand : IRequest<Result<UpdateGenreVM>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, Result<UpdateGenreVM>>
    {
        private readonly IGenericRepository<Genre> _genreGenericRepository;
        private readonly ILogger _logger;

        public UpdateGenreCommandHandler(IGenericRepository<Genre> genreGenericRepository, ILogger<UpdateGenreCommandHandler> logger)
        {
            _genreGenericRepository = genreGenericRepository;
            _logger = logger;
        }

        public async Task<Result<UpdateGenreVM>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на обновление игры с ID = {request.Id}");
            Genre? oldGenre = await _genreGenericRepository.GetByIdAsync(request.Id, cancellationToken);

            if (oldGenre is null)
            {
                string message = $"Не удалось найти жанр с ID = {request.Id}";
                _logger.LogWarning(message);
                return Result.Failure<UpdateGenreVM>(message);
            }

            Genre newGenre = new Genre { Id = request.Id, Name = request.Name };

            var validateResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(newGenre, new ValidationContext(newGenre), validateResults))
            {
                string error = validateResults.First().ErrorMessage!;
                _logger.LogWarning($"Не пройдена валижация: {error}");
                return Result.Failure<UpdateGenreVM>(error);
            }

            await _genreGenericRepository.UpdateAsync(newGenre, cancellationToken);

            UpdateGenreVM updateGameVm = new UpdateGenreVM
            {
                OldGenre = new UpdateGenreDTO
                {
                    Id = oldGenre.Id,
                    Name = oldGenre.Name,
                },
                NewGenre = new UpdateGenreDTO
                {
                    Id = newGenre.Id,
                    Name = newGenre.Name,
                }
            };
            _logger.LogInformation($"Обновление завершено {updateGameVm}");
            return updateGameVm;
        }
    }
}
