using CSharpFunctionalExtensions;
using GamesLibrary.Application.Features.Genres.Commands.CreateGenre;
using GamesLibrary.Application.Features.Genres.Commands.DeleteGenre;
using GamesLibrary.Application.Features.Genres.Commands.UpdateGenre;
using GamesLibrary.Application.Features.Genres.Queries.GetAllGenres;
using GamesLibrary.Application.Features.Genres.Queries.GetGenreById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesLibrary.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<GetAllGenresDTO>?>> GetAllAsync()
        {
            Result<IEnumerable<GetAllGenresDTO>> result = await _mediator.Send(new GetAllGenresQuery());
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<GetGenreByIdDTO>> GetById(Guid id)
        {
            Result<GetGenreByIdDTO> result = await _mediator.Send(new GetGenreByIdQuery(id));
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Guid>> Create(CreateGenreCommand genre)
        {
            Result<Guid> result = await _mediator.Send(genre);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<UpdateGenreVM>> Update(UpdateGenreCommand genre)
        {
            Result<UpdateGenreVM> result = await _mediator.Send(genre);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> DeleteGame(Guid id)
        {
            Result result = await _mediator.Send(new DeleteGenreCommand(id));
            if (result.IsFailure) return NotFound(result.Error);
            return Ok();
        }
    }
}
