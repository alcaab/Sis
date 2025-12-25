using Desyco.Dms.Application.Evaluations.Commands;
using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Dms.Application.Evaluations.Queries;
using Desyco.Dms.Domain.Evaluations;
using Desyco.Iam.Contracts.Permissions;
using Desyco.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrima.Core.Query;
using Scrima.OData.AspNetCore;

namespace Desyco.Dms.Web.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/evaluation-periods")]
public class EvaluationPeriodsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.EvaluationPeriods.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<QueryResult<EvaluationPeriodDto>>> GetAll(ODataQuery<EvaluationPeriodEntity> queryOptions,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllEvaluationPeriodsQuery(queryOptions), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = Permissions.EvaluationPeriods.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EvaluationPeriodDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetEvaluationPeriodByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.EvaluationPeriods.Write)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EvaluationPeriodDto>> Create([FromBody] EvaluationPeriodDto evaluationPeriod,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateEvaluationPeriodCommand(evaluationPeriod), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = Permissions.EvaluationPeriods.Write)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] EvaluationPeriodDto evaluationPeriod,
        CancellationToken cancellationToken)
    {
        if (id != evaluationPeriod.Id)
        {
            return BadRequest("Id in URL and request body do not match.");
        }

        await mediator.Send(new UpdateEvaluationPeriodCommand(evaluationPeriod), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = Permissions.EvaluationPeriods.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteEvaluationPeriodCommand(id), cancellationToken);
        return NoContent();
    }
}
