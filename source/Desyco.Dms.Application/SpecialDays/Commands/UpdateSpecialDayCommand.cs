using Desyco.Mediator.Contracts;
using Desyco.Dms.Application.SpecialDays.DTOs;

namespace Desyco.Dms.Application.SpecialDays.Commands;

public record UpdateSpecialDayCommand(SpecialDayDto SpecialDay) : IRequest<SpecialDayDto>;
