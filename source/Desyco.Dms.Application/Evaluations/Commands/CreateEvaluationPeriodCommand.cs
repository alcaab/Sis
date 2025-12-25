using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.Evaluations.Commands;

public record CreateEvaluationPeriodCommand(EvaluationPeriodDto EvaluationPeriod) : IRequest<EvaluationPeriodDto>;
