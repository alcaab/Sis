using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Invoices.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Invoices.Repositories;

public class InvoiceDetailRepository(ApplicationDbContext context) : RepositoryBase<InvoiceDetailEntity, int>(context), IInvoiceDetailRepository;