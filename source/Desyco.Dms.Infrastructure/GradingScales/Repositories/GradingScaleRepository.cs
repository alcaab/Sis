using Desyco.Dms.Domain.GradingScales.Interfaces;
using Desyco.Dms.Domain.GradingScales;

namespace Desyco.Dms.Infrastructure.GradingScales.Repositories;

public class GradingScaleRepository(ApplicationDbContext context) : RepositoryBase<GradingScaleEntity, int>(context), IGradingScaleRepository;
