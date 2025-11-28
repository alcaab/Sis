using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Payments;

namespace Desyco.Dms.Domain.Payments.Interfaces;

public interface IPaymentDetailRepository : IRepositoryBase<PaymentDetailEntity, int>;
