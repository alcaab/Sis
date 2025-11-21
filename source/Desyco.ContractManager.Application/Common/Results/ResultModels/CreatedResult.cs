namespace Desyco.ContractManager.Application.Common.Results.ResultModels;

public record CreatedResult<T>(string Location, T Value) : Result<T>(Value);
