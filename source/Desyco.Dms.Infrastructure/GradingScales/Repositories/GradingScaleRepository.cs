using Desyco.Dms.Domain.GradingScales.Interfaces;
using Desyco.Dms.Domain.GradingScales;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.GradingScales.Repositories;

public class GradingScaleRepository(ApplicationDbContext context) : RepositoryBase<GradingScaleEntity, int>(context), IGradingScaleRepository;
