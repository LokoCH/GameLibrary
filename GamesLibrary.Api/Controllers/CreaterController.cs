using CSharpFunctionalExtensions;
using GamesLibrary.Application.Features.Creaters.Commands.CreateCreater;
using GamesLibrary.Application.Features.Creaters.Commands.DeleteCreater;
using GamesLibrary.Application.Features.Creaters.Commands.UpdateCreater;
using GamesLibrary.Application.Features.Creaters.Queries.GetAllCreaters;
using GamesLibrary.Application.Features.Creaters.Queries.GetCreaterById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesLibrary.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreaterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreaterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<GetAllCreatersDTO>>> GetAllAsync()
        {
            Result<IEnumerable<GetAllCreatersDTO>> result = await _mediator.Send(new GetAllCreatersQuery());
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<GetCreaterByIdDTO>> GetById(Guid id)
        {
            Result<GetCreaterByIdDTO> result = await _mediator.Send(new GetCreaterByIdQuery(id));
            if (result.IsFailure) return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Guid>> Create(CreateCreaterCommand creater)
        {
            Result<Guid> result = await _mediator.Send(creater);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<UpdateCreaterVM>> Update(UpdateCreaterCommand creater)
        {
            Result<UpdateCreaterVM> result = await _mediator.Send(creater);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            Result result = await _mediator.Send(new DeleteCreaterCommand(id));
            if (result.IsFailure) return NotFound(result.Error);
            return Ok();
        }
    }
}
