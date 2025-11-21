using Desyco.Dms.Application.Common.Results;
using Desyco.Dms.Application.Common.Results.Error;

namespace Desyco.Dms.Application.Common.RequestPipelines.Behaviors;

internal static class BehaviorResult
{
    private static readonly Type ResultGenericType = typeof(Result<>);

    public static bool IsResult<TResponse>()
    {
        var type = typeof(TResponse);

        return typeof(Result).IsAssignableFrom(type) ||
               (type.IsGenericType && type.GetGenericTypeDefinition() == ResultGenericType);
    }

    public static TResponse Fail<TResponse>(Error error)
    {
        var responseType = typeof(TResponse);

        if (responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == ResultGenericType)
        {
            var innerType = responseType.GetGenericArguments()[0];
            var resultType = typeof(Result<>).MakeGenericType(innerType);
            var instance = Activator.CreateInstance(resultType, error)!;
            return (TResponse)instance;
        }

        if (typeof(Result).IsAssignableFrom(responseType))
        {
            return (TResponse)(object)new Result(error);
        }

        throw new InvalidOperationException(
            $"The response type '{responseType}' does not implement Result or Result<T>.");
    }
}
