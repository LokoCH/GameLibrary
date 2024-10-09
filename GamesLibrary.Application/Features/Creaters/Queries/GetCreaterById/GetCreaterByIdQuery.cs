using CSharpFunctionalExtensions;
using GamesLibrary.Application.Features.Games.Commands.CreateGame;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Creaters.Queries.GetCreaterById
{
    public record GetCreaterByIdQuery(Guid Id) : IRequest<Result<GetCreaterByIdDTO>>;

    public class GetCreaterByIdQueryHandler : IRequestHandler<GetCreaterByIdQuery, Result<GetCreaterByIdDTO>>
    {
        private readonly IGenericRepository<Creater> _createrGenericRepository;
        private readonly ILogger _logger;

        public GetCreaterByIdQueryHandler(IGenericRepository<Creater> repository, ILogger<GetCreaterByIdQueryHandler> logger)
        {
            _createrGenericRepository = repository;
            _logger = logger;
        }

        public async Task<Result<GetCreaterByIdDTO>> Handle(GetCreaterByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Запрос студии с ID = {request.Id}");
            Creater? creater = await _createrGenericRepository.GetByIdAsync(request.Id, cancellationToken);

            if (creater is null)
            {
                string message = "Данные не найдены";
                _logger.LogWarning(message);
                return Result.Failure<GetCreaterByIdDTO>(message);
            }

            var result = new GetCreaterByIdDTO { Id = creater.Id, Name = creater.Name };
            _logger.LogInformation($"Ответ: {JsonSerializer.Serialize(result)}");

            return result;
        }
    }
}
