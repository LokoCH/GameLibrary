using CSharpFunctionalExtensions;
using GamesLibrary.Api.Middlewares.Exceptions;
using GamesLibrary.Application.Features.Games.Commands.CreateGame;
using GamesLibrary.Application.Features.Games.Commands.DeleteGame;
using GamesLibrary.Application.Features.Games.Commands.UpdateGame;
using GamesLibrary.Application.Features.Games.Queries.GetAllGames;
using GamesLibrary.Application.Features.Games.Queries.GetGameByGenre;
using GamesLibrary.Application.Features.Games.Queries.GetGameById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace GamesLibrary.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<GetAllGamesDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetAllGamesDTO>>> GetAllAsync(bool? isFullInfo = null)
        {
            Result<IEnumerable<GetAllGamesDTO>> result = await _mediator.Send(new GetAllGamesQuery(isFullInfo));
            return Ok(result.Value);
        }

        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(GetGameByIdDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetGameByIdDTO>> GetById(Guid id, bool? isFullInfo = null)
        {
            Result<GetGameByIdDTO> result = await _mediator.Send(new GetGameByIdQuery(id, isFullInfo));
            if (result.IsFailure) return NotFound(new ErrorModel { StatusCode = StatusCodes.Status404NotFound, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpGet("[action]/{genreid}")]
        [ProducesResponseType(typeof(GetGamesByGenreDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GetGamesByGenreDTO>>> GetByGenre(Guid genreid)
        {
            Result<IEnumerable<GetGamesByGenreDTO>> result = await _mediator.Send(new GetGamesByGenreQuery(genreid));
            if (result.IsFailure) return NotFound(new ErrorModel { StatusCode = StatusCodes.Status404NotFound, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create(CreateGameCommand game)
        {
            Result<Guid> result = await _mediator.Send(game);
            if (result.IsFailure) return BadRequest(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(typeof(UpdateGameDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateGameDTO>> Update(UpdateGameCommand game)
        {
            Result<UpdateGameDTO> result = await _mediator.Send(game);
            if (result.IsFailure) return BadRequest(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            Result result = await _mediator.Send(new DeleteGameCommand(id));
            if (result.IsFailure) return NotFound(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok();
        }
    }
}
