using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class PaymentConceptTypeSeeder(ApplicationDbContext context, ILogger<PaymentConceptTypeSeeder> logger) 
    : EnumEntitySeeder<PaymentConceptTypeEntity, PaymentConceptType>(context, logger)
{
    protected override PaymentConceptTypeEntity CreateEntity(PaymentConceptType id) => new() { Id = id };
}
