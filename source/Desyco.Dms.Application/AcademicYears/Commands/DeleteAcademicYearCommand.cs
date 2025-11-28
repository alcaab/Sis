using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.AcademicYears.Commands;

public record DeleteAcademicYearCommand(int Id) : IRequest;
