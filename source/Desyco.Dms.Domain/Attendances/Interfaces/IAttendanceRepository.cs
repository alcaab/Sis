using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Attendances;

namespace Desyco.Dms.Domain.Attendances.Interfaces;

public interface IAttendanceRepository : IRepositoryBase<AttendanceEntity, int>;
