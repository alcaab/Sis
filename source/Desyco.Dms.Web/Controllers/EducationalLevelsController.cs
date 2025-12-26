using Desyco.Dms.Application.EducationalLevels.Commands;
using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Dms.Application.EducationalLevels.Queries;
using Desyco.Dms.Domain.EducationalLevels;
using Desyco.Iam.Contracts.Permissions;
using Desyco.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrima.Core.Query;
using Scrima.OData.AspNetCore;

namespace Desyco.Dms.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/educational-levels")]
public class EducationalLevelsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.EducationalLevels.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<QueryResult<EducationalLevelDto>>> GetAll(ODataQuery<EducationalLevelEntity> queryOptions,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEducationalLevelListQuery(queryOptions), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = Permissions.EducationalLevels.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EducationalLevelDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEducationalLevelByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.EducationalLevels.Write)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EducationalLevelDto>> Create([FromBody] EducationalLevelDto educationalLevel,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateEducationalLevelCommand(educationalLevel), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = Permissions.EducationalLevels.Write)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EducationalLevelDto>> Update(int id, [FromBody] EducationalLevelDto educationalLevel,
        CancellationToken cancellationToken)
    {
        if (id != educationalLevel.Id)
        {
            return BadRequest("Id in URL and request body do not match.");
        }

        var result = await mediator.Send(new UpdateEducationalLevelCommand(educationalLevel), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = Permissions.EducationalLevels.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteEducationalLevelCommand(id), cancellationToken);
        return NoContent();
    }
}
