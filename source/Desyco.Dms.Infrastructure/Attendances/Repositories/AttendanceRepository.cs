using Desyco.Dms.Domain.Attendances.Interfaces;
using Desyco.Dms.Domain.Attendances;

namespace Desyco.Dms.Infrastructure.Attendances.Repositories;

public class AttendanceRepository(ApplicationDbContext context) : RepositoryBase<AttendanceEntity, int>(context), IAttendanceRepository;