using CSharpFunctionalExtensions;
using GamesLibrary.Api.Middlewares.Exceptions;
using GamesLibrary.Application.Features.Creaters.Commands.CreateCreater;
using GamesLibrary.Application.Features.Creaters.Commands.DeleteCreater;
using GamesLibrary.Application.Features.Creaters.Commands.UpdateCreater;
using GamesLibrary.Application.Features.Creaters.Queries.GetAllCreaters;
using GamesLibrary.Application.Features.Creaters.Queries.GetCreaterById;
using GamesLibrary.Application.Features.Games.Commands.UpdateGame;
using GamesLibrary.Application.Features.Games.Queries.GetAllGames;
using GamesLibrary.Application.Features.Games.Queries.GetGameById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GamesLibrary.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public class CreaterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreaterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<GetAllCreatersDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetAllCreatersDTO>>> GetAllAsync()
        {
            Result<IEnumerable<GetAllCreatersDTO>> result = await _mediator.Send(new GetAllCreatersQuery());
            return Ok(result.Value);
        }

        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(GetCreaterByIdDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetCreaterByIdDTO>> GetById(Guid id)
        {
            Result<GetCreaterByIdDTO> result = await _mediator.Send(new GetCreaterByIdQuery(id));
            if (result.IsFailure) return NotFound(new ErrorModel { StatusCode = StatusCodes.Status404NotFound, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create(CreateCreaterCommand creater)
        {
            Result<Guid> result = await _mediator.Send(creater);
            if (result.IsFailure) return BadRequest(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(typeof(UpdateCreaterDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateCreaterDTO>> Update(UpdateCreaterCommand creater)
        {
            Result<UpdateCreaterDTO> result = await _mediator.Send(creater);
            if (result.IsFailure) return BadRequest(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            Result result = await _mediator.Send(new DeleteCreaterCommand(id));
            if (result.IsFailure) return NotFound(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok();
        }
    }
}
