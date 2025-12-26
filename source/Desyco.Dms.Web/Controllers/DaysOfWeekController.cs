using Desyco.Dms.Application.DaysOfWeek;
using Desyco.Dms.Application.DaysOfWeek.DTOs;
using Desyco.Dms.Application.DaysOfWeek.Queries;
using Desyco.Iam.Contracts.Permissions;
using Desyco.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desyco.Dms.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/days-of-week")]
public class DaysOfWeekController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    //[Authorize(Policy = Permissions.DaysOfWeek.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DayOfWeekDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllDaysOfWeekQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.DaysOfWeek.Write)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DayOfWeekDto>> Update(DayOfWeek id, [FromBody] UpdateDayOfWeekCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Id in URL and request body do not match.");
        }

        try
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
