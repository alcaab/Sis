using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Domain.Payments.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Payments.Repositories;

public class PaymentDetailRepository(ApplicationDbContext context) : RepositoryBase<PaymentDetailEntity, int>(context), IPaymentDetailRepository;