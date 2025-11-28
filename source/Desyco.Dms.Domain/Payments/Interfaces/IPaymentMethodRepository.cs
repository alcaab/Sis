using Desyco.Dms.Domain.Common.Interfaces;

namespace Desyco.Dms.Domain.Payments.Interfaces;

public interface IPaymentMethodRepository : IRepositoryBase<PaymentMethodEntity, PaymentMethod>;
