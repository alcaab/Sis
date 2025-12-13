using Desyco.Dms.Domain.ClassSchedules.Interfaces;
using Desyco.Dms.Domain.ClassSchedules;

namespace Desyco.Dms.Infrastructure.ClassSchedules.Repositories;

public class ClassScheduleRepository(ApplicationDbContext context) : RepositoryBase<ClassScheduleEntity, int>(context), IClassScheduleRepository;
