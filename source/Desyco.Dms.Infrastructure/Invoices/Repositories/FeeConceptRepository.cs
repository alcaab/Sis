using Desyco.Dms.Domain.Invoices.FeeConcepts;
using Desyco.Dms.Domain.Invoices.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Invoices.Repositories;

public class FeeConceptRepository(ApplicationDbContext context) : RepositoryBase<FeeConceptEntity, int>(context), IFeeConceptRepository;