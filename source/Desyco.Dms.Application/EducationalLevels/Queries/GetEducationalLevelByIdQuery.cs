using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.EducationalLevels.Queries;

public record GetEducationalLevelByIdQuery(int Id) : IRequest<EducationalLevelDto>;
