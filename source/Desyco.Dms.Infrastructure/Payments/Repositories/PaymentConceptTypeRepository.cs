using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Domain.Payments.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Payments.Repositories;

public class PaymentConceptTypeRepository(ApplicationDbContext context) : RepositoryBase<PaymentConceptTypeEntity, PaymentConceptType>(context), IPaymentConceptTypeRepository;