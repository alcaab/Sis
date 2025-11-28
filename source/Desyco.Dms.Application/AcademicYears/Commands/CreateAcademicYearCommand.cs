using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.AcademicYears.DTOs;

namespace Desyco.Dms.Application.AcademicYears.Commands;

public record CreateAcademicYearCommand(AcademicYearDto AcademicYear) : IRequest<AcademicYearDto>;