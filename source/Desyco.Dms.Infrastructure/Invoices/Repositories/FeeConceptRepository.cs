using Desyco.Dms.Domain.Invoices.FeeConcepts;
using Desyco.Dms.Domain.Invoices.Interfaces;

namespace Desyco.Dms.Infrastructure.Invoices.Repositories;

public class FeeConceptRepository(ApplicationDbContext context) : RepositoryBase<FeeConceptEntity, int>(context), IFeeConceptRepository;