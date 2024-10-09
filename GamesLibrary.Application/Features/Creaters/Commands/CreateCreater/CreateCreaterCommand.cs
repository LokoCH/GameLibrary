using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Creaters.Commands.CreateCreater
{
    public record CreateCreaterCommand() : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class CreateCreaterCommandHandler : IRequestHandler<CreateCreaterCommand, Result<Guid>>
    {
        private readonly IGenericRepository<Creater> _createrGenericRepository;
        private readonly ILogger _logger;

        public CreateCreaterCommandHandler(IGenericRepository<Creater> createrGenericRepository, ILogger<CreateCreaterCommandHandler> logger)
        {
            _createrGenericRepository = createrGenericRepository;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateCreaterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на добавление студии: {request}");

            Creater creater = new Creater
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(creater, new ValidationContext(creater), validationResults, true))
            {
                string error = validationResults.First().ErrorMessage!;
                _logger.LogWarning($"Ошибка валидации: {error}");
                return Result.Failure<Guid>(error);
            }

            Creater newCreater = await _createrGenericRepository.CreateAsync(creater, cancellationToken);
            _logger.LogInformation($"Добавлена студия: {newCreater}");

            return newCreater.Id;
        }
    }
}
