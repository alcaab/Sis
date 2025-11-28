using Desyco.Dms.Domain.Invoices.Interfaces;
using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Invoices.FeeConcepts;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Invoices.Repositories;

public class InvoiceRepository(ApplicationDbContext context) : RepositoryBase<InvoiceEntity, int>(context), IInvoiceRepository;

public class InvoiceDetailRepository(ApplicationDbContext context) : RepositoryBase<InvoiceDetailEntity, int>(context), IInvoiceDetailRepository;

public class InvoiceStatusRepository(ApplicationDbContext context) : RepositoryBase<InvoiceStatusEntity, InvoiceStatus>(context), IInvoiceStatusRepository;

public class FeeConceptRepository(ApplicationDbContext context) : RepositoryBase<FeeConceptEntity, int>(context), IFeeConceptRepository;
