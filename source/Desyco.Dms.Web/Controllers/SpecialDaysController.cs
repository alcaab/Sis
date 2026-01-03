using Desyco.Dms.Application.SpecialDays.Commands;
using Desyco.Dms.Application.SpecialDays.DTOs;
using Desyco.Dms.Application.SpecialDays.Queries;
using Desyco.Dms.Domain.SpecialDays;
using Desyco.Iam.Contracts.Permissions;
using Desyco.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrima.Core.Query;
using Scrima.OData.AspNetCore;

namespace Desyco.Dms.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/special-days")]
public class SpecialDaysController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.SpecialDays.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<QueryResult<SpecialDayDto>>> GetAll(ODataQuery<SpecialDayEntity> queryOptions,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSpecialDaysListQuery(queryOptions), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = Permissions.SpecialDays.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpecialDayDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSpecialDayByIdQuery(id), cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("by-period/{evaluationPeriodId:int}")]
    [Authorize(Policy = Permissions.SpecialDays.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SpecialDayDto>>> GetByPeriod(int evaluationPeriodId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSpecialDaysByPeriodQuery(evaluationPeriodId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("by-year/{academicYearId:int}")]
    [Authorize(Policy = Permissions.SpecialDays.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SpecialDayDto>>> GetByAcademicYear(int academicYearId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSpecialDaysByAcademicYearQuery(academicYearId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.SpecialDays.Write)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SpecialDayDto>> Create([FromBody] SpecialDayDto dto,
        CancellationToken cancellationToken)
    {
        var command = new CreateSpecialDayCommand(dto);
        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = Permissions.SpecialDays.Write)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpecialDayDto>> Update(int id, [FromBody] SpecialDayDto dto,
        CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id in URL and request body do not match.");
        }

        var command = new UpdateSpecialDayCommand(dto);
        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = Permissions.SpecialDays.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteSpecialDayCommand(id), cancellationToken);
        return NoContent();
    }
}
