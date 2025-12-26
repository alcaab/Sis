using Desyco.Dms.Application.EducationalLevels.DTOs;
using Desyco.Mediator.Contracts;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.EducationalLevels.Queries;

public record GetEducationalLevelListQuery(QueryOptions QueryOptions) : IRequest<QueryResult<EducationalLevelDto>>;
