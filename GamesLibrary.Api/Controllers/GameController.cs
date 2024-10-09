using CSharpFunctionalExtensions;
using GamesLibrary.Application.Features.Games.Commands.CreateGame;
using GamesLibrary.Application.Features.Games.Commands.DeleteGame;
using GamesLibrary.Application.Features.Games.Commands.UpdateGame;
using GamesLibrary.Application.Features.Games.Queries.GetAllGames;
using GamesLibrary.Application.Features.Games.Queries.GetGameByGenre;
using GamesLibrary.Application.Features.Games.Queries.GetGameById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesLibrary.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<GetAllGamesDTO>?>> GetAllAsync(bool? isFullInfo = null)
        {
            Result<IEnumerable<GetAllGamesDTO>> result = await _mediator.Send(new GetAllGamesQuery(isFullInfo));
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<GetGameByIdDTO>> GetById(Guid id, bool? isFullInfo = null)
        {
            Result<GetGameByIdDTO> result = await _mediator.Send(new GetGameByIdQuery(id, isFullInfo));
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("[action]/{genreid}")]
        public async Task<ActionResult<IEnumerable<GetGamesByGenreDTO>>> GetByGenre(Guid genreid)
        {
            Result<IEnumerable<GetGamesByGenreDTO>> result = await _mediator.Send(new GetGamesByGenreQuery(genreid));
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Guid>> Create(CreateGameCommand game)
        {
            Result<Guid> result = await _mediator.Send(game);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<UpdateGameVM>> Update(UpdateGameCommand game)
        {
            Result<UpdateGameVM> result = await _mediator.Send(game);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            Result result = await _mediator.Send(new DeleteGameCommand(id));
            if (result.IsFailure) return NotFound(result.Error);
            return Ok();
        }
    }
}
