using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Payments;

namespace Desyco.Dms.Domain.Payments.Interfaces;

public interface IPaymentMethodRepository : IRepositoryBase<PaymentMethodEntity, PaymentMethod>;
