using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.SpecialDays.DTOs;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.SpecialDays.Queries;

public record GetSpecialDaysListQuery(QueryOptions QueryOptions) : IRequest<QueryResult<SpecialDayDto>>;
