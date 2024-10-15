using CSharpFunctionalExtensions;
using GamesLibrary.Api.Middlewares.Exceptions;
using GamesLibrary.Application.Features.Creaters.Queries.GetAllCreaters;
using GamesLibrary.Application.Features.Creaters.Queries.GetCreaterById;
using GamesLibrary.Application.Features.Games.Commands.UpdateGame;
using GamesLibrary.Application.Features.Genres.Commands.CreateGenre;
using GamesLibrary.Application.Features.Genres.Commands.DeleteGenre;
using GamesLibrary.Application.Features.Genres.Commands.UpdateGenre;
using GamesLibrary.Application.Features.Genres.Queries.GetAllGenres;
using GamesLibrary.Application.Features.Genres.Queries.GetGenreById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace GamesLibrary.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<GetAllGenresDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetAllGenresDTO>?>> GetAllAsync()
        {
            Result<IEnumerable<GetAllGenresDTO>> result = await _mediator.Send(new GetAllGenresQuery());
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("[action]/{id}")]
        [ProducesResponseType(typeof(GetGenreByIdDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetGenreByIdDTO>> GetById(Guid id)
        {
            Result<GetGenreByIdDTO> result = await _mediator.Send(new GetGenreByIdQuery(id));
            if (result.IsFailure) return NotFound(new ErrorModel { StatusCode = StatusCodes.Status404NotFound, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create(CreateGenreCommand genre)
        {
            Result<Guid> result = await _mediator.Send(genre);
            if (result.IsFailure) return BadRequest(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpPut("[action]")]
        [ProducesResponseType(typeof(UpdateGenreDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateGenreDTO>> Update(UpdateGenreCommand genre)
        {
            Result<UpdateGenreDTO> result = await _mediator.Send(genre);
            if (result.IsFailure) return BadRequest(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok(result.Value);
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteGame(Guid id)
        {
            Result result = await _mediator.Send(new DeleteGenreCommand(id));
            if (result.IsFailure) return NotFound(new ErrorModel { StatusCode = StatusCodes.Status400BadRequest, Message = result.Error });
            return Ok();
        }
    }
}
