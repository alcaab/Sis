using Desyco.Dms.Domain.ClassSchedules.Interfaces;
using Desyco.Dms.Domain.ClassSchedules;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.ClassSchedules.Repositories;

public class ClassScheduleRepository(ApplicationDbContext context) : RepositoryBase<ClassScheduleEntity, int>(context), IClassScheduleRepository;
