using Desyco.Dms.Domain.EducationalLevels.Interfaces;
using Desyco.Dms.Domain.EducationalLevels;

namespace Desyco.Dms.Infrastructure.EducationalLevels.Repositories;

public class EducationalLevelRepository(ApplicationDbContext context) : RepositoryBase<EducationalLevelEntity, int>(context), IEducationalLevelRepository;
