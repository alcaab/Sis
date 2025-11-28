using Desyco.Dms.Domain.ScaleLevels.Interfaces;
using Desyco.Dms.Domain.ScaleLevels;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.ScaleLevels.Repositories;

public class ScaleLevelRepository(ApplicationDbContext context) : RepositoryBase<ScaleLevelEntity, int>(context), IScaleLevelRepository;
