using Desyco.Dms.Application.AcademicYears.Commands;
using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Application.AcademicYears.Queries;
using Desyco.Dms.Domain.AcademicYears;
using Desyco.Iam.Contracts.Permissions; // Added
using Desyco.Mediator;
using Microsoft.AspNetCore.Authorization; // Added
using Microsoft.AspNetCore.Mvc;
using Scrima.Core.Query;
using Scrima.OData.AspNetCore;

namespace Desyco.Dms.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/academic-years")]
public class AcademicYearsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.AcademicYears.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<QueryResult<AcademicYearDto>>> GetAll(ODataQuery<AcademicYearEntity> queryOptions,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllAcademicYearsQuery(queryOptions), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = Permissions.AcademicYears.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AcademicYearDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAcademicYearByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AcademicYears.Write)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AcademicYearDto>> Create([FromBody] AcademicYearDto dto,
        CancellationToken cancellationToken)
    {
        var command = new CreateAcademicYearCommand(dto);
        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = Permissions.AcademicYears.Write)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AcademicYearDto>> Update(int id, [FromBody] AcademicYearDto dto,
        CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id in URL and request body do not match.");
        }

        var command = new UpdateAcademicYearCommand(dto);
        try
        {
            var result = await mediator.Send(command, cancellationToken);

            return Ok(result);
        }
        catch (Exception ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            // TODO: Replace with a more specific exception handling for NotFoundException
            // once the command handlers are updated to throw specific exceptions.
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = Permissions.AcademicYears.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await mediator.Send(new DeleteAcademicYearCommand(id), cancellationToken);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            // TODO: Replace with a more specific exception handling for NotFoundException
            // once the command handlers are updated to throw specific exceptions.
            return NotFound();
        }
    }
}
