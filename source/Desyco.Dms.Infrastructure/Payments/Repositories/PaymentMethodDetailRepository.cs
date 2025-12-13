using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Domain.Payments.Interfaces;

namespace Desyco.Dms.Infrastructure.Payments.Repositories;

public class PaymentMethodDetailRepository(ApplicationDbContext context) : RepositoryBase<PaymentMethodDetailEntity, int>(context), IPaymentMethodDetailRepository;