using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.AcademicYears.DTOs;

namespace Desyco.Dms.Application.AcademicYears.Queries;

public record GetAllAcademicYearsQuery : IRequest<List<AcademicYearDto>>;
