using Desyco.Dms.Domain.Invoices.Interfaces;
using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Invoices.Repositories;

public class InvoiceRepository(ApplicationDbContext context) : RepositoryBase<InvoiceEntity, int>(context), IInvoiceRepository;