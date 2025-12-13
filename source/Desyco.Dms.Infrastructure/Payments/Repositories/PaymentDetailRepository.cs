using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Domain.Payments.Interfaces;

namespace Desyco.Dms.Infrastructure.Payments.Repositories;

public class PaymentDetailRepository(ApplicationDbContext context) : RepositoryBase<PaymentDetailEntity, int>(context), IPaymentDetailRepository;