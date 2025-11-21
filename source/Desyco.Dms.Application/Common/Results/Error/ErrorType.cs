namespace Desyco.Dms.Application.Common.Results.Error;

public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    DependencyNotFound,
}
