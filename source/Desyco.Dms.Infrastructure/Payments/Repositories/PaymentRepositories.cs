using Desyco.Dms.Domain.Payments.Interfaces;
using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Payments.Repositories;

public class PaymentRepository(ApplicationDbContext context) : RepositoryBase<PaymentEntity, int>(context), IPaymentRepository;

public class PaymentDetailRepository(ApplicationDbContext context) : RepositoryBase<PaymentDetailEntity, int>(context), IPaymentDetailRepository;

public class PaymentMethodRepository(ApplicationDbContext context) : RepositoryBase<PaymentMethodEntity, PaymentMethod>(context), IPaymentMethodRepository;

public class PaymentMethodDetailRepository(ApplicationDbContext context) : RepositoryBase<PaymentMethodDetailEntity, int>(context), IPaymentMethodDetailRepository;

public class PaymentConceptTypeRepository(ApplicationDbContext context) : RepositoryBase<PaymentConceptTypeEntity, PaymentConceptType>(context), IPaymentConceptTypeRepository;
