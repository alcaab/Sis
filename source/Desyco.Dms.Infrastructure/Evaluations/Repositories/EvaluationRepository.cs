using Desyco.Dms.Domain.Evaluations.Interfaces;
using Desyco.Dms.Domain.Evaluations;

namespace Desyco.Dms.Infrastructure.Evaluations.Repositories;

public class EvaluationRepository(ApplicationDbContext context) : RepositoryBase<EvaluationEntity, int>(context), IEvaluationRepository;