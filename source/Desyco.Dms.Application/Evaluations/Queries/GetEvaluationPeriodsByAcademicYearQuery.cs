using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.Evaluations.Queries;

public record GetEvaluationPeriodsByAcademicYearQuery(int AcademicYearId) : IRequest<List<EvaluationPeriodViewDto>>;
