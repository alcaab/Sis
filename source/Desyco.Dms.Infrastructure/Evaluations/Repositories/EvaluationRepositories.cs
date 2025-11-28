using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Dms.Domain.Evaluations;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Evaluations.Repositories;

public class EvaluationRepository(ApplicationDbContext context) : RepositoryBase<EvaluationEntity, int>(context), IEvaluationRepository;

public class EvaluationPeriodRepository(ApplicationDbContext context) : RepositoryBase<EvaluationPeriodEntity, int>(context), IEvaluationPeriodRepository;
