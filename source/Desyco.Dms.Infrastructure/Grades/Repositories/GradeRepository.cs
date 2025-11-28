using Desyco.Dms.Domain.Grades.Interfaces;
using Desyco.Dms.Domain.Grades;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Grades.Repositories;

public class GradeRepository(ApplicationDbContext context) : RepositoryBase<GradeEntity, int>(context), IGradeRepository;
