using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class InvoiceStatusSeeder(ApplicationDbContext context, ILogger<InvoiceStatusSeeder> logger) 
    : EnumEntitySeeder<InvoiceStatusEntity, InvoiceStatus>(context, logger)
{
    protected override InvoiceStatusEntity CreateEntity(InvoiceStatus id) => new() { Id = id };
}
