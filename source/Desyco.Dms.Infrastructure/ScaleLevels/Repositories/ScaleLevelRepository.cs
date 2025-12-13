using Desyco.Dms.Domain.ScaleLevels.Interfaces;
using Desyco.Dms.Domain.ScaleLevels;

namespace Desyco.Dms.Infrastructure.ScaleLevels.Repositories;

public class ScaleLevelRepository(ApplicationDbContext context) : RepositoryBase<ScaleLevelEntity, int>(context), IScaleLevelRepository;
