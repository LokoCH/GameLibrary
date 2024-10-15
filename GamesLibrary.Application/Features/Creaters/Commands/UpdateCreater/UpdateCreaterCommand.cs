using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Application.Features.Creaters.Commands.UpdateCreater
{
    public record UpdateCreaterCommand : IRequest<Result<UpdateCreaterDTO>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateCreaterCommandHandler : IRequestHandler<UpdateCreaterCommand, Result<UpdateCreaterDTO>>
    {
        private readonly IGenericRepository<Creater> _createrGenericRepository;
        private readonly ILogger _logger;

        public UpdateCreaterCommandHandler(IGenericRepository<Creater> createrGenericRepository, ILogger<UpdateCreaterCommandHandler> logger)
        {
            _createrGenericRepository = createrGenericRepository;
            _logger = logger;
        }

        public async Task<Result<UpdateCreaterDTO>> Handle(UpdateCreaterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на обновление студии с ID = {request.Id}");
            Creater? oldCreater = await _createrGenericRepository.GetByIdAsync(request.Id, cancellationToken);

            if (oldCreater is null)
            {
                string message = $"Не удалось найти студию с ID = {request.Id}";
                _logger.LogWarning(message);
                return Result.Failure<UpdateCreaterDTO>(message);
            }

            oldCreater.Id = request.Id;
            oldCreater.Name = request.Name;

            var validateResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(oldCreater, new ValidationContext(oldCreater), validateResults))
            {
                string error = validateResults.First().ErrorMessage!;
                _logger.LogWarning($"Не пройдена валидация: {error}");
                return Result.Failure<UpdateCreaterDTO>(error);
            }

            await _createrGenericRepository.UpdateAsync(oldCreater, cancellationToken);

            UpdateCreaterDTO updateCreaterVm = new UpdateCreaterDTO
            {
                Id = oldCreater.Id,
                Name = oldCreater.Name
            };
            _logger.LogInformation($"Обновление завершено {updateCreaterVm}");
            return updateCreaterVm;
        }
    }
}
