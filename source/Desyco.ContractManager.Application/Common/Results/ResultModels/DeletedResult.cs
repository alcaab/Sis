namespace Desyco.ContractManager.Application.Common.Results.ResultModels;

// ReSharper disable NotAccessedPositionalProperty.Global
public record DeletedResult(int? Requested, int Successful, int NotFound) : Result;
