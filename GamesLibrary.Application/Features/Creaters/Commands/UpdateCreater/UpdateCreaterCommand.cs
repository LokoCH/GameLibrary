using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace GamesLibrary.Application.Features.Creaters.Commands.UpdateCreater
{
    public record UpdateCreaterCommand : IRequest<Result<UpdateCreaterVM>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateCreaterCommandHandler : IRequestHandler<UpdateCreaterCommand, Result<UpdateCreaterVM>>
    {
        private readonly IGenericRepository<Creater> _createrGenericRepository;
        private readonly ILogger _logger;

        public UpdateCreaterCommandHandler(IGenericRepository<Creater> createrGenericRepository, ILogger<UpdateCreaterCommandHandler> logger)
        {
            _createrGenericRepository = createrGenericRepository;
            _logger = logger;
        }

        public async Task<Result<UpdateCreaterVM>> Handle(UpdateCreaterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос на обновление студии с ID = {request.Id}");
            Creater? oldCreater = await _createrGenericRepository.GetByIdAsync(request.Id, cancellationToken);

            if (oldCreater is null)
            {
                string message = $"Не удалось найти студию с ID = {request.Id}";
                _logger.LogWarning(message);
                return Result.Failure<UpdateCreaterVM>(message);
            }

            Creater newCreater = new Creater { Id = request.Id, Name = request.Name };

            var validateResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(newCreater, new ValidationContext(newCreater), validateResults))
            {
                string error = validateResults.First().ErrorMessage!;
                _logger.LogWarning($"Не пройдена валидация: {error}");
                return Result.Failure<UpdateCreaterVM>(error);
            }

            await _createrGenericRepository.UpdateAsync(newCreater, cancellationToken);

            UpdateCreaterVM updateCreaterVm = new UpdateCreaterVM
            {
                OldCreater = new UpdateCreaterDTO
                {
                    Id = oldCreater.Id,
                    Name = oldCreater.Name
                },
                NewCreater = new UpdateCreaterDTO
                {
                    Id = newCreater.Id,
                    Name = newCreater.Name
                }
            };
            _logger.LogInformation($"Обновление завершено {updateCreaterVm}");
            return updateCreaterVm;
        }
    }
}
