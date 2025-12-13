using Desyco.Dms.Domain.Attendances;
using Desyco.Dms.Domain.Attendances.Interfaces;

namespace Desyco.Dms.Infrastructure.Attendances.Repositories;

public class AttendanceStatusRepository(ApplicationDbContext context) : RepositoryBase<AttendanceStatusEntity, AttendanceStatus>(context), IAttendanceStatusRepository;