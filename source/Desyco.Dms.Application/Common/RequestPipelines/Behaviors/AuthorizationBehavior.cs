using System.Reflection;
using Desyco.Dms.Application.Common.Auth;
using Desyco.Dms.Application.Common.Results.Error;
using Microsoft.Extensions.Logging;

namespace Desyco.Dms.Application.Common.RequestPipelines.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(
    IEnumerable<IAuthGuard<TRequest>> authGuards,
    IUserContext userContext,
    ILogger<AuthorizationBehavior<TRequest, TResponse>> logger
)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IAuthGuard<TRequest>[] _authGuards = authGuards.ToArray();

    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> nextBehavior,
        CancellationToken cancellationToken
    )
    {
        var permissionAttribute = typeof(TRequest).GetCustomAttribute<RequirePermissionAttribute>();

        if (permissionAttribute is not null && permissionAttribute.Permissions.Length > 0)
        {
            logger.CheckingPermissionsForRequest(typeof(TRequest).Name);

            if (!userContext.IsAuthenticated)
            {
                logger.UserIsNotAuthenticated();
                return BehaviorResult.Fail<TResponse>(Error.Unauthorized());
            }

            logger.UserAuthenticatedAs(userContext.Subject, userContext.IsClientUser());

            var requireAllPermissions = permissionAttribute.RequireAll;
            var hasPermissions = false;
            foreach (var permission in permissionAttribute.Permissions)
            {
                var hasClaim = userContext.HasClaim(userContext.ClaimType, permission);

                switch (hasClaim)
                {
                    case false when requireAllPermissions:
                        logger.UserDoesNotHavePermission(userContext.Subject, permission);
                        return BehaviorResult.Fail<TResponse>(Error.Forbidden());
                    case false:
                        continue;
                }

                hasPermissions = true;

                if (!requireAllPermissions)
                {
                    break;
                }
            }

            if (!hasPermissions)
            {
                logger.UserDoesNotHavePermissions(userContext.Subject);
                return BehaviorResult.Fail<TResponse>(Error.Forbidden());
            }
        }

        if (_authGuards.Length == 0)
        {
            logger.NoAuthGuardsFound(typeof(TRequest).Name);
            return await nextBehavior();
        }

        foreach (var guard in _authGuards)
        {
            var result = guard.CheckPermissions(request, userContext);
            if (!result.IsFailure)
            {
                continue;
            }

            logger.UserDoesNotHavePermissions(userContext.Subject, result.ErrorMessage);
            return BehaviorResult.Fail<TResponse>(Error.Forbidden());
        }

        logger.AuthorizationSuccess(typeof(TRequest).Name, userContext.Subject);
        return await nextBehavior();
    }
}
