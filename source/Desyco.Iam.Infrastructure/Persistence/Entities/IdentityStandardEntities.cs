using Microsoft.AspNetCore.Identity;

namespace Desyco.Iam.Infrastructure.Persistence.Entities;

public class ApplicationUserClaim : IdentityUserClaim<Guid> { }

public class ApplicationRoleClaim : IdentityRoleClaim<Guid> { }

public class ApplicationUserRole : IdentityUserRole<Guid> { }

public class ApplicationUserLogin : IdentityUserLogin<Guid> { }

public class ApplicationUserToken : IdentityUserToken<Guid> { }
