using Desyco.Dms.Domain.Guardians.Interfaces;
using Desyco.Dms.Domain.Guardians;

namespace Desyco.Dms.Infrastructure.Guardians.Repositories;

public class GuardianRepository(ApplicationDbContext context) : RepositoryBase<GuardianEntity, int>(context), IGuardianRepository;
