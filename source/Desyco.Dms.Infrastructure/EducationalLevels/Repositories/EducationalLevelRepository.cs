using Desyco.Dms.Domain.EducationalLevels.Interfaces;
using Desyco.Dms.Domain.EducationalLevels;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.EducationalLevels.Repositories;

public class EducationalLevelRepository(ApplicationDbContext context) : RepositoryBase<EducationalLevelEntity, int>(context), IEducationalLevelRepository;
