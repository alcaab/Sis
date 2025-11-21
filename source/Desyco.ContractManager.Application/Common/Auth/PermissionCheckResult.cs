namespace Desyco.ContractManager.Application.Common.Auth;

public sealed class PermissionCheckResult
{
    public static PermissionCheckResult Success() => new(true, null);

    public static PermissionCheckResult Fail(string errorMessage) => new(false, errorMessage);

    private PermissionCheckResult(bool isSuccess, string? errorMessage)
    {
        ErrorMessage = errorMessage;
        IsSuccess = isSuccess;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string? ErrorMessage { get; }
}
