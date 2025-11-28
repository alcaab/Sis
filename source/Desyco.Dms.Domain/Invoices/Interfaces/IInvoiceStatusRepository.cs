using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Invoices.FeeConcepts;

namespace Desyco.Dms.Domain.Invoices.Interfaces;

public interface IInvoiceStatusRepository : IRepositoryBase<InvoiceStatusEntity, InvoiceStatus>;
