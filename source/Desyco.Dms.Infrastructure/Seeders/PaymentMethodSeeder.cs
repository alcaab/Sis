using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Infrastructure.Common.Seeding;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Infrastructure.Seeders;

public class PaymentMethodSeeder(ApplicationDbContext context, ILogger<PaymentMethodSeeder> logger) 
    : EnumEntitySeeder<PaymentMethodEntity, PaymentMethod>(context, logger)
{
    protected override PaymentMethodEntity CreateEntity(PaymentMethod id) => new() { Id = id };
}
