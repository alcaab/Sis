using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.Evaluations.Commands;

public record DeleteEvaluationPeriodCommand(int Id) : IRequest;
