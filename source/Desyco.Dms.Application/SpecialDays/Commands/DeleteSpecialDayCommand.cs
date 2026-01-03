using Desyco.Mediator.Contracts;

namespace Desyco.Dms.Application.SpecialDays.Commands;

public record DeleteSpecialDayCommand(int Id) : IRequest;
