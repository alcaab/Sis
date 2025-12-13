using Desyco.Dms.Domain.Payments.Interfaces;
using Desyco.Dms.Domain.Payments;

namespace Desyco.Dms.Infrastructure.Payments.Repositories;

public class PaymentRepository(ApplicationDbContext context) : RepositoryBase<PaymentEntity, int>(context), IPaymentRepository;