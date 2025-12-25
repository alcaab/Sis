using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.Evaluations.DTOs;

namespace Desyco.Dms.Application.Evaluations.Queries;

public record GetEvaluationPeriodByIdQuery(int Id) : IRequest<EvaluationPeriodDto>;
