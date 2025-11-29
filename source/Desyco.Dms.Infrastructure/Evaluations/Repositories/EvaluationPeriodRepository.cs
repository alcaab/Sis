using Desyco.Dms.Domain.Evaluations;
using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Evaluations.Repositories;

public class EvaluationPeriodRepository(ApplicationDbContext context) : RepositoryBase<EvaluationPeriodEntity, int>(context), IEvaluationPeriodRepository;