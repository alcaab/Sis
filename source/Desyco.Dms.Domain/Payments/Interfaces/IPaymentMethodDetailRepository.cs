using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Payments;

namespace Desyco.Dms.Domain.Payments.Interfaces;

public interface IPaymentMethodDetailRepository : IRepositoryBase<PaymentMethodDetailEntity, int>;
