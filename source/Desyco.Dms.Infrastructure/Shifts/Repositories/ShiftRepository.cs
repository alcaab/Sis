using Desyco.Dms.Domain.Shifts.Interfaces;
using Desyco.Dms.Domain.Shifts;

namespace Desyco.Dms.Infrastructure.Shifts.Repositories;

public class ShiftRepository(ApplicationDbContext context) : RepositoryBase<ShiftEntity, ShiftType>(context), IShiftRepository;
