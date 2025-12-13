using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Invoices.Interfaces;

namespace Desyco.Dms.Infrastructure.Invoices.Repositories;

public class InvoiceStatusRepository(ApplicationDbContext context) : RepositoryBase<InvoiceStatusEntity, InvoiceStatus>(context), IInvoiceStatusRepository;