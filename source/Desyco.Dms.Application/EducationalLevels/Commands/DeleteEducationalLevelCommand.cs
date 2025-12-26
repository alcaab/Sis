using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.EducationalLevels.Commands;

public record DeleteEducationalLevelCommand(int Id) : IRequest;
