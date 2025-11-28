using Desyco.Dms.Domain.Common.Interfaces;

namespace Desyco.Dms.Domain.Payments.Interfaces;

public interface IPaymentRepository : IRepositoryBase<PaymentEntity, int>;
