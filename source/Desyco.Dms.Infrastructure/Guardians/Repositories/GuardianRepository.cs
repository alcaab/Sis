using Desyco.Dms.Domain.Guardians.Interfaces;
using Desyco.Dms.Domain.Guardians;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Guardians.Repositories;

public class GuardianRepository(ApplicationDbContext context) : RepositoryBase<GuardianEntity, int>(context), IGuardianRepository;
