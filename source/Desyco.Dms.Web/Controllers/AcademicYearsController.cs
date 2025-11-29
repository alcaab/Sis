using Desyco.Dms.Application.AcademicYears.Commands;
using Desyco.Dms.Application.AcademicYears.DTOs;
using Desyco.Dms.Application.AcademicYears.Queries;
using Desyco.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Desyco.Dms.Web.Controllers;

[ApiController]
[Route("api/academic-years")]
public class AcademicYearsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AcademicYearDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllAcademicYearsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AcademicYearDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAcademicYearByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AcademicYearDto>> Create([FromBody] AcademicYearDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateAcademicYearCommand(dto);
        var result = await mediator.Send(command, cancellationToken);
        // Assuming the command returns the created DTO, which includes its generated ID.
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AcademicYearDto>> Update(int id, [FromBody] AcademicYearDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id in URL and request body do not match.");
        }

        var command = new UpdateAcademicYearCommand(dto);
        try
        {
            var result = await mediator.Send(command, cancellationToken);
            // The handler for UpdateAcademicYearCommand returns the updated DTO.
            // Returning 200 OK with the updated resource is a valid REST practice.
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await mediator.Send(new DeleteAcademicYearCommand(id), cancellationToken);
            return NoContent(); // 204 No Content for successful deletion.
        }
        catch (Exception ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            // TODO: Replace with a more specific exception handling for NotFoundException
            // once the command handlers are updated to throw specific exceptions.
            return NotFound();
        }
    }
}
