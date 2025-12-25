using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.Evaluations.Commands;

public record UpdateEvaluationPeriodCommand(EvaluationPeriodDto EvaluationPeriod) : IRequest<EvaluationPeriodDto>;
