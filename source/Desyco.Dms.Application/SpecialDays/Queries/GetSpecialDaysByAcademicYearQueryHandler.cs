using Desyco.Dms.Application.SpecialDays.DTOs;
using Desyco.Dms.Application.SpecialDays.Mappers;
using Desyco.Dms.Domain.SpecialDays.Interfaces;
using Desyco.Mediator;

namespace Desyco.Dms.Application.SpecialDays.Queries;

public class GetSpecialDaysByAcademicYearQueryHandler(ISpecialDayRepository specialDayRepository, SpecialDayMapper mapper)
    : IRequestHandler<GetSpecialDaysByAcademicYearQuery, IReadOnlyList<SpecialDayEducationalLevelDto>>
{
    public async Task<IReadOnlyList<SpecialDayEducationalLevelDto>> Handle(GetSpecialDaysByAcademicYearQuery request, CancellationToken cancellationToken = default)
    {
        var list = await specialDayRepository.GetByAcademicYearIdAsync(request.AcademicYearId, cancellationToken);

        var result = list
            .GroupBy(x => new
            {
                x.EducationalLevelEntity.Id,
                x.EducationalLevelEntity.Name
            })
            .Select(levelGroup => new SpecialDayEducationalLevelDto
            {
                Id = levelGroup.Key.Id,
                Name = levelGroup.Key.Name,

                Periods = levelGroup
                    .GroupBy(x => new
                    {
                        x.EvaluationPeriodEntity.Id,
                        x.EvaluationPeriodEntity.Name,
                        x.EvaluationPeriodEntity.StartDate,
                        x.EvaluationPeriodEntity.EndDate,
                        x.EvaluationPeriodEntity.Sequence
                    })
                    .Select(periodGroup => new SpecialDayEvaluationPeriodDto
                    {
                        Id = periodGroup.Key.Id,
                        Name = periodGroup.Key.Name,
                        StartDate = periodGroup.Key.StartDate,
                        EndDate = periodGroup.Key.EndDate,
                        Sequence = periodGroup.Key.Sequence,

                        Days = periodGroup
                            .Where(x => x.SpecialDayEntity != null)
                            .Select(x => new SpecialDayDto
                            {
                                Id = x.SpecialDayEntity!.Id,
                                Name = x.SpecialDayEntity.Name,
                                Description = x.SpecialDayEntity.Description,
                                Date = x.SpecialDayEntity.Date,
                                Type = x.SpecialDayEntity.Type,
                                OpenTime = x.SpecialDayEntity.OpenTime,
                                StartTime = x.SpecialDayEntity.StartTime,
                                EndTime = x.SpecialDayEntity.EndTime,
                                CloseTime = x.SpecialDayEntity.CloseTime
                            })
                            .ToList()
                    })
                    .OrderBy(p => p.Sequence)
                    .ToList()
            })
            .ToList();

        return result;
    }
}
