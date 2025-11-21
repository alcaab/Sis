using System.ComponentModel.DataAnnotations;
using Desyco.ContractManager.Application.Common.Results.Error;
using Microsoft.Extensions.Logging;

namespace Desyco.ContractManager.Application.Common.RequestPipelines.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    ILogger<ValidationBehavior<TRequest, TResponse>> logger,
    IServiceProvider serviceProvider
)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(request, serviceProvider, null);

        var isValid = Validator.TryValidateObject(
            request,
            context,
            validationResults,
            validateAllProperties: true
        );

        if (!isValid)
        {
            logger.ValidationFailed(typeof(TRequest).Name, validationResults.Count);

            var error = Error.Validation(validationResults);
            return BehaviorResult.Fail<TResponse>(error);
        }

        logger.ValidationPassed(typeof(TRequest).Name);

        return await next();
    }
}
