using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GamesLibrary.Application.Features.Creaters.Commands.DeleteCreater
{
    public record DeleteCreaterCommand(Guid id) : IRequest<Result>;

    public class DeleteCreaterCommandHandler : IRequestHandler<DeleteCreaterCommand, Result>
    {
        private readonly IGenericRepository<Creater> _createrGenericRepository;
        private readonly ILogger _logger;

        public DeleteCreaterCommandHandler(IGenericRepository<Creater> createrGenericRepository, ILogger<DeleteCreaterCommandHandler> logger)
        {
            _createrGenericRepository = createrGenericRepository;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteCreaterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Удаление студии с ID = {request.id}");
            Creater? creater = await _createrGenericRepository.GetByIdAsync(request.id, cancellationToken);
            
            if (creater is null)
            {
                string message = $"Не удалось найти студию с ID = {request.id}";
                _logger.LogWarning(message);
                return Result.Failure(message);
            }

            await _createrGenericRepository.DeleteAsync(creater, cancellationToken);
            _logger.LogInformation($"Удаление студии с ID = {request.id} завершено");
            return Result.Success();
        }
    }
}
