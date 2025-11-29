using Desyco.Dms.Domain.Attendances;
using Desyco.Dms.Domain.Attendances.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Attendances.Repositories;

public class AttendanceStatusRepository(ApplicationDbContext context) : RepositoryBase<AttendanceStatusEntity, AttendanceStatus>(context), IAttendanceStatusRepository;