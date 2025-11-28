using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Common.Entities;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Common.Repositories;

public class DayOfWeekRepository(ApplicationDbContext context) : RepositoryBase<DayOfWeekEntity, DayOfWeek>(context), IDayOfWeekRepository;

public class RelationshipRepository(ApplicationDbContext context) : RepositoryBase<RelationshipEntity, int>(context), IRelationshipRepository;

public class SpecialityRepository(ApplicationDbContext context) : RepositoryBase<SpecialityEntity, int>(context), ISpecialityRepository;
