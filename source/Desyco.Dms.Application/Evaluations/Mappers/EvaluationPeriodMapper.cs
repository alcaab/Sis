using Desyco.Dms.Application.Evaluations.DTOs;
using Desyco.Dms.Domain.Evaluations;
using Riok.Mapperly.Abstractions;
using Scrima.Core.Query;

namespace Desyco.Dms.Application.Evaluations.Mappers;

[Mapper]
public partial class EvaluationPeriodMapper
{
    public partial EvaluationPeriodDto Map(EvaluationPeriodEntity entity);
    
    public partial EvaluationPeriodEntity Map(EvaluationPeriodDto dto);
    
    public partial void Map(EvaluationPeriodDto dto, EvaluationPeriodEntity entity);
    
    public partial EvaluationPeriodViewDto MapToView(EvaluationPeriodEntity entity);
    
    public partial QueryResult<EvaluationPeriodDto> Map(QueryResult<EvaluationPeriodEntity> queryResult);
}