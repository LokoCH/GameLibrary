using CSharpFunctionalExtensions;
using GamesLibrary.Application.Repositories;
using GamesLibrary.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GamesLibrary.Application.Features.Creaters.Queries.GetAllCreaters
{
    public record GetAllCreatersQuery() : IRequest<Result<IEnumerable<GetAllCreatersDTO>>>;

    public class GetAllCreatersQueryHandler : IRequestHandler<GetAllCreatersQuery, Result<IEnumerable<GetAllCreatersDTO>>>
    {
        private readonly IGenericRepository<Creater> _createrGenericRepository;
        private readonly ILogger _logger;

        public GetAllCreatersQueryHandler(IGenericRepository<Creater> createrGenericRepository, ILogger<GetAllCreatersQueryHandler> logger)
        {
            _createrGenericRepository = createrGenericRepository;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<GetAllCreatersDTO>>> Handle(GetAllCreatersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Запрос на информацию о всех студиях");
            IEnumerable<GetAllCreatersDTO> result = [];
            IEnumerable<Creater>? creaters = await _createrGenericRepository.GetAllAsync(cancellationToken);

            if (creaters is not null && creaters.Any())
            {
                result = creaters.Select(creater => new GetAllCreatersDTO()
                {
                    Id = creater.Id,
                    Name = creater.Name
                });
            }

            _logger.LogInformation($"Ответ: {JsonSerializer.Serialize(result)}");
            return Result.Success(result);
        }
    }
}
