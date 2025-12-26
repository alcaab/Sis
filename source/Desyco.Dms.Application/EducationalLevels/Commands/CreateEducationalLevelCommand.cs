using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.EducationalLevels.Commands;

public record CreateEducationalLevelCommand(EducationalLevelDto EducationalLevel) : IRequest<EducationalLevelDto>;
