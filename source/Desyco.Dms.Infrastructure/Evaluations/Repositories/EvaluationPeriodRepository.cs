using Desyco.Dms.Domain.Evaluations;
using Desyco.Dms.Domain.Evaluations.Interfaces;

namespace Desyco.Dms.Infrastructure.Evaluations.Repositories;

public class EvaluationPeriodRepository(ApplicationDbContext context) : RepositoryBase<EvaluationPeriodEntity, int>(context), IEvaluationPeriodRepository;