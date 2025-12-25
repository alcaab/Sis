using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.Evaluations.DTOs;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.Evaluations.Queries;

public record GetAllEvaluationPeriodsQuery(QueryOptions QueryOptions) : IRequest<QueryResult<EvaluationPeriodDto>>;
