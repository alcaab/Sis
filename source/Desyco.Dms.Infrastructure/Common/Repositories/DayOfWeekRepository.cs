using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Common.Entities;

namespace Desyco.Dms.Infrastructure.Common.Repositories;

public class DayOfWeekRepository(ApplicationDbContext context) : RepositoryBase<DayOfWeekEntity, DayOfWeek>(context), IDayOfWeekRepository;