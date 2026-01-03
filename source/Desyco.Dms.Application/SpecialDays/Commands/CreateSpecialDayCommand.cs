using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.SpecialDays.DTOs;

namespace Desyco.Dms.Application.SpecialDays.Commands;

public record CreateSpecialDayCommand(SpecialDayDto SpecialDay) : IRequest<SpecialDayDto>;
