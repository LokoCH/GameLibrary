using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Genres.Commands.CreateGenre
{
    public record CreateGenreCommand(): IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, Result<Guid>>
    {
        private readonly IGenericRepository<Genre> _genreGenericRepository;
        private readonly ILogger _logger;

        public CreateGenreCommandHandler(IGenericRepository<Genre> genreGenericRepository, ILogger<CreateGenreCommandHandler> logger)
        {
            _genreGenericRepository = genreGenericRepository;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на добавление жанра: {request}");

            Genre genre = new Genre
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
            };
            
            var validationResults = new List<ValidationResult>();
            if(!Validator.TryValidateObject(genre, new ValidationContext(genre), validationResults, true))
            {
                string error = validationResults.First().ErrorMessage!;
                _logger.LogWarning($"Ошибка валидации: {error}");
                return Result.Failure<Guid>(error);
            }

            Genre newGenre = await _genreGenericRepository.CreateAsync(genre, cancellationToken);
            _logger.LogInformation($"Добавлен жанр: {newGenre}");

            return newGenre.Id;
        }
    }
}
