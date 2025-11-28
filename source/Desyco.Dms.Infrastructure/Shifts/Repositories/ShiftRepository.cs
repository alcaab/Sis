using Desyco.Dms.Domain.Shifts.Interfaces;
using Desyco.Dms.Domain.Shifts;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Shifts.Repositories;

public class ShiftRepository(ApplicationDbContext context) : RepositoryBase<ShiftEntity, ShiftType>(context), IShiftRepository;
