using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Invoices.Interfaces;

namespace Desyco.Dms.Infrastructure.Invoices.Repositories;

public class InvoiceDetailRepository(ApplicationDbContext context) : RepositoryBase<InvoiceDetailEntity, int>(context), IInvoiceDetailRepository;