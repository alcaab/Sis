using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.AcademicYears.DTOs;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.AcademicYears.Queries;

public record GetAllAcademicYearsQuery(QueryOptions QueryOptions) : IRequest<QueryResult<AcademicYearDto>>;
