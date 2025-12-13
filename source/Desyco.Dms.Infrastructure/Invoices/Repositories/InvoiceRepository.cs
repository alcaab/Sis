using Desyco.Dms.Domain.Invoices.Interfaces;
using Desyco.Dms.Domain.Invoices;

namespace Desyco.Dms.Infrastructure.Invoices.Repositories;

public class InvoiceRepository(ApplicationDbContext context) : RepositoryBase<InvoiceEntity, int>(context), IInvoiceRepository;